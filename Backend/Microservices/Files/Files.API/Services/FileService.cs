using AutoMapper;
using Files.API.DataTransferObjects;
using Files.API.Exceptions;
using Files.API.Model;
using Files.API.Services.Contracts;
using MassTransit;
using MassTransitModels.File;
using Microsoft.EntityFrameworkCore;
using Models.File;
using File = Files.API.Model.File;

namespace Files.API.Services;

public class FileService : IFileService
{
    private readonly ApplicationDatabaseContext _context;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRequestClient<RetrieveFile> _client;
    private readonly ILogger<FileService> _logger;

    public FileService(
        ApplicationDatabaseContext context,
        IMapper mapper,
        IPublishEndpoint publishEndpoint,
        IRequestClient<RetrieveFile> client,
        ILogger<FileService> logger)
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _client = client;
        _logger = logger;
        _logger.LogInformation("File Service is called");
    }
    public async Task<File> GetFileAsync(Guid userId, Guid fileId)
    {
        var file = await _context.Files
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == fileId);
        
        if (file is null)
        {
            throw new InvalidFileIdBadRequestException(fileId);
        }

        return file;
    }
    public async Task<ICollection<File>> GetFilesByUserIdAsync(Guid userId)
    {
        var files = await _context.Files
            .Where(x => x.UserId == userId)
            .ToListAsync();

        return files;
    }
    public async Task<File> CreateFileAsync(CreateFileDto dto)
    {
        var entity = _mapper.Map<File>(dto);

        _context.Files.Add(entity);
        
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"File with id: {entity.Id} created");

        return entity;
    }
    public async Task UpdateFileAsync(Guid userId, UpdateFileDto dto)
    {
        var file = await GetFileAsync(userId, dto.Id);

        if (file is null)
        {
            throw new InvalidFileIdBadRequestException(dto.Id);
        }

        _mapper.Map(dto, file);

        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"File with id: {file.Id} was updated");
    }

    public async Task DeleteFileAsync(Guid userId, Guid fileId)
    {
        var file = await GetFileAsync(userId, fileId);

        if (file == null)
        {
            throw new InvalidFileIdBadRequestException(fileId);
        }

        await _publishEndpoint.Publish<FileDeleted>(new
        {
            Name = file.Id.ToString(),
            Extension = file.Extension
        });
        
        _context.Files.Remove(file);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"File with id: {file.Id} was deleted");
    }
    public async Task UploadFileAsync(FormFileDto dto, Guid userId)
    {
        var createFileDto = _mapper.Map<CreateFileDto>(dto);
        createFileDto.UserId = userId;
        var file = await CreateFileAsync(createFileDto);
        await using var stream = new MemoryStream();
        await dto.File.CopyToAsync(stream);
        var bytes = stream.ToArray();
        await _publishEndpoint.Publish<FileCreated>(new
        {
            Bytes = bytes,
            Name = file.Id.ToString(),
            Extension = file.Extension,
        });
        _logger.LogInformation("File created event published to RabbitMQ");
    }

    public async Task<DownloadFileDto> DownloadFileAsync(Guid userId, Guid id)
    {
        var file = await GetFileAsync(userId, id);
        
        _logger.LogInformation("Retrieve request to RabbitMQ");
        
        var response = await _client.GetResponse<RetrieveFileResult>(new
        {
            Name = file.Id.ToString(),
            Extension = file.Extension
        });;

        return new()
        {
            Bytes = response.Message.Bytes,
            Name = file.Name,
            Extension = file.Extension
        };
    }
}