using AutoMapper;
using Files.Application.Common.Exceptions;
using MassTransit;
using MassTransitModels.File;
using Microsoft.EntityFrameworkCore;
using Models.File;
using Files.Application.Features.File.DataTransferObjects;
using Files.Infrastructure.Persistence;
using Files.Infrastructure.Persistence.RepositoryManagers;
using Microsoft.Extensions.Logging;

namespace Files.Application.Features.File.Services;
public class FileService : IFileService
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRequestClient<RetrieveFile> _client;
    private readonly ILogger<FileService> _logger;
    private readonly IRepositoryManager _repositoryManager;

    public FileService(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        IPublishEndpoint publishEndpoint,
        IRequestClient<RetrieveFile> client,
        ILogger<FileService> logger)
    {
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _client = client;
        _logger = logger;
        _repositoryManager = repositoryManager;
        _logger.LogInformation("File Service is called");
    }
    public async Task<FileDto> GetFileAsync(Guid userId, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await FindFileAsync(userId, fileId, cancellationToken);
        var dto = _mapper.Map<FileDto>(file); 
        return dto;
    }
    public async Task<ICollection<FileDto>> GetFilesAsync(
        Guid userId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var files = await _repositoryManager
            .FileRepository
            .FindByCondition(x => x.UserId == userId, trackChanges)
            .ToListAsync(cancellationToken);

        return _mapper.Map<ICollection<FileDto>>(files);
    }
    public async Task<FileDto> CreateFileAsync(
        CreateFileDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Domain.Entities.File.File>(dto);

        await _repositoryManager.FileRepository.CreateAsync(entity, cancellationToken);
        
        await _repositoryManager.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"File with id: {entity.Id} created");

        return _mapper.Map<FileDto>(entity);
    }
    public async Task UpdateFileAsync(
        Guid userId, UpdateFileDto dto, CancellationToken cancellationToken = default)
    {
        var file = await GetFileAsync(userId, dto.Id, cancellationToken);

        if (file is null)
        {
            throw new InvalidFileIdBadRequestException(dto.Id);
        }

        _mapper.Map(dto, file);

        await _repositoryManager.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"File with id: {file.Id} was updated");
    }

    public async Task DeleteFileAsync(Guid userId, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await FindFileAsync(userId, fileId);

        if (file == null)
        {
            throw new InvalidFileIdBadRequestException(fileId);
        }

        await _publishEndpoint.Publish<FileDeleted>(new
        {
            Name = file.Id.ToString(),
            Extension = file.Extension
        });
        
        _repositoryManager.FileRepository.Remove(file);
        await _repositoryManager.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"File with id: {file.Id} was deleted");
    }
    public async Task UploadFileAsync(FormFileDto dto, Guid userId, CancellationToken cancellationToken = default)
    {
        var createFileDto = _mapper.Map<CreateFileDto>(dto);
        createFileDto.UserId = userId;
        var file = await CreateFileAsync(createFileDto, cancellationToken);
        await using var stream = new MemoryStream();
        await dto.File.CopyToAsync(stream, cancellationToken);
        var bytes = stream.ToArray();
        await _publishEndpoint.Publish<FileCreated>(new
        {
            Bytes = bytes,
            Name = file.Id.ToString(),
            Extension = file.Extension,
        }, cancellationToken);
        _logger.LogInformation("File created event published to RabbitMQ");
    }

    public async Task<DownloadFileDto> DownloadFileAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var file = await GetFileAsync(userId, id, cancellationToken);
        
        _logger.LogInformation("Retrieve request to RabbitMQ");
        
        var response = await _client.GetResponse<RetrieveFileResult>(new
        {
            Name = file.Id.ToString(),
            Extension = file.Extension
        }, cancellationToken);;

        return new()
        {
            Bytes = response.Message.Bytes,
            Name = file.Name,
            Extension = file.Extension
        };
    }
     
    private async Task<Domain.Entities.File.File> FindFileAsync(
        Guid userId, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await _repositoryManager
            .FileRepository
            .FindSingleAsync(x => x.UserId == userId && x.Id == fileId, cancellationToken);
        
        if (file is null)
        {
            throw new InvalidFileIdBadRequestException(fileId);
        }

        return file;
    }
}