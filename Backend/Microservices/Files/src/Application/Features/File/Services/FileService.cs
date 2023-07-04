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
    public async Task<FileDto> GetFileAsync(
        Guid userId, 
        Guid? directoryId,
        Guid fileId, 
        CancellationToken cancellationToken = default)
    {
        var file = await FindFileAsync(userId, directoryId, fileId, false, cancellationToken);
        var dto = _mapper.Map<FileDto>(file); 
        return dto;
    }

    public async Task<ICollection<FileDto>> GetFilesAsync(
        Guid userId, 
        Guid? directoryId, 
        bool trackChanges, 
        CancellationToken cancellationToken = default)
    {
        var files = await _repositoryManager
            .FileRepository
            .FindByCondition(x => x.UserId == userId 
                                  && x.DirectoryId == directoryId, trackChanges)
            .ToListAsync(cancellationToken);

        return _mapper.Map<ICollection<FileDto>>(files);
    }
    private async Task<FileDto> CreateFileAsync(
        Guid userId,
        CreateFileDto dto, 
        CancellationToken cancellationToken = default)
    {
        if (dto.DirectoryId.HasValue)
        {
            var directory = await _repositoryManager
                .DirectoryRepository
                .FindSingleAsync(x => x.UserId == userId && x.Id == dto.DirectoryId.Value, cancellationToken);
            if (directory is null)
            {
                throw new InvalidDirectoryIdBadRequestException(dto.DirectoryId.Value);
            }
        }
        
        var entity = _mapper.Map<Domain.Entities.File.File>(dto);
        entity.UserId = userId;
        await _repositoryManager.FileRepository.CreateAsync(entity, cancellationToken);
        await _repositoryManager.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"File with id: {entity.Id} created");

        return _mapper.Map<FileDto>(entity);
    }
    public async Task<FileDto> UpdateFileAsync(
        Guid userId, UpdateFileDto dto, CancellationToken cancellationToken = default)
    {
        var file = await FindFileAsync(userId, dto.DirectoryId, dto.Id, true, cancellationToken);
        _mapper.Map(dto, file);
        file.Updated = DateTime.Now;

        await _repositoryManager.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"File with id: {file.Id} was updated");
        
        var resultDto = _mapper.Map<FileDto>(file);
        return resultDto;
    }

    public async Task DeleteFileAsync(
        Guid userId, 
        Guid? directoryId,
        Guid fileId, 
        CancellationToken cancellationToken = default)
    {
        var file = await FindFileAsync(userId, directoryId, fileId, false, cancellationToken);

        if (file == null)
        {
            throw new InvalidFileIdBadRequestException(fileId);
        }

        await _publishEndpoint.Publish<FileDeleted>(new
        {
            Name = file.Id.ToString(),
            Extension = file.Extension
        }, cancellationToken);
        
        _repositoryManager.FileRepository.Remove(file);
        await _repositoryManager.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"File with id: {file.Id} was deleted");
    }
    public async Task<FileDto> UploadFileAsync(Guid userId, FormFileDto dto, CancellationToken cancellationToken = default)
    {
        var createFileDto = _mapper.Map<CreateFileDto>(dto);
        var file = await CreateFileAsync(userId, createFileDto, cancellationToken);
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

        return file;
    }

    public async Task<DownloadFileDto> DownloadFileAsync(
        Guid userId,
        Guid? directoryId,
        Guid id, CancellationToken cancellationToken = default)
    {
        var file = await GetFileAsync(userId, directoryId, id, cancellationToken);
        
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
        Guid userId,
        Guid? directoryId,
        Guid fileId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        Domain.Entities.File.File? file = null;
        if (directoryId.HasValue)
        {
            var directory = await _repositoryManager
                .DirectoryRepository
                .FindAll(trackChanges)
                .Include(x => x.Files)
                .FirstOrDefaultAsync(x => x.UserId == userId 
                                          && x.Id == directoryId, cancellationToken);

            if (directory is null)
            {
                throw new InvalidDirectoryIdBadRequestException(directoryId.Value);
            }
        
            file = directory.Files.FirstOrDefault(x => x.Id == fileId);
        }
        else
        {
            file = await _repositoryManager
                .FileRepository
                .FindSingleAsync(x => x.UserId == userId 
                                      && !x.DirectoryId.HasValue 
                                      && x.Id == fileId, cancellationToken);
        }
        if (file is null)
        {
            throw new InvalidFileIdBadRequestException(fileId);
        }

        return file;
    }
}