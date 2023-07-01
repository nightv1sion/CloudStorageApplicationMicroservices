using AutoMapper;
using Files.Application.Common.Exceptions;
using Files.Application.Features.File;
using Files.Application.Features.File.Services;
using Files.Infrastructure.Persistence;
using Files.Infrastructure.Persistence.RepositoryManagers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.File;
using Moq;
using Moq.EntityFrameworkCore;
using File = Files.Domain.Entities.File.File;
namespace Application.UnitTests.Services;

public class FileServiceTests
{
    /*private readonly IFileService _service;
    private readonly IMapper _mapper;
    private readonly Mock<IPublishEndpoint> _publishEndpoint;
    private readonly Mock<IRequestClient<RetrieveFile>> _client;
    private readonly Mock<ILogger<FileService>> _logger;
    private readonly Mock<IRepositoryManager> _repositoryManager;

    public FileServiceTests()
    {
        _mapper = new MapperConfiguration(x =>
        {
            x.AddProfile<MappingProfile>();
        }).CreateMapper();
        _publishEndpoint = new Mock<IPublishEndpoint>();
        _client = new Mock<IRequestClient<RetrieveFile>>();
        _logger = new Mock<ILogger<FileService>>();
        _repositoryManager = new Mock<IRepositoryManager>();
        _service = new FileService(
            _repositoryManager.Object,_mapper, _publishEndpoint.Object, _client.Object, _logger.Object);
    }

    [Fact]
    public async Task GetFile_NotExistingFileId_ThrowsInvalidFileIdBadRequest()
    {
        _context.Setup<DbSet<File>>(expression: x =>
            x.Files)
            .ReturnsDbSet(new List<File>());

        var func = async () => await _service.GetFileAsync(Guid.NewGuid(), Guid.NewGuid());

        await Assert.ThrowsAsync<InvalidFileIdBadRequestException>(func);
    }
    [Fact]
    public async Task GetFile_ExistingFileId_ReturnsValidFile()
    {
        var fileId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var file = new File()
        {
            Id = fileId,
            UserId = userId,
        };
        
        _context.Setup<DbSet<File>>(
                expression: x => x.Files)
            .ReturnsDbSet(new List<File> { file });

        var result = await _service.GetFileAsync(userId, fileId);
        
        Assert.Equal(file.Id, result.Id);
    }
    [Fact]
    public async Task GetFilesByUserId_UserIdWithFiles_ReturnsValidFiles()
    {
        var fileId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var validFiles = new List<File>()
        {
            new() { UserId = userId },
            new() { UserId = userId },
            new() { UserId = userId },
            new() { UserId = userId },
        };

        var invalidFiles = new List<File>()
        {
            new() { UserId = Guid.NewGuid() },
            new() { UserId = Guid.NewGuid() },
            new() { UserId = Guid.NewGuid() },
        };

        var files = validFiles.Concat(invalidFiles).ToList();
        
        _context.Setup<DbSet<File>>(
                expression: x => x.Files)
            .ReturnsDbSet(files);

        var result = await _service.GetFilesAsync(userId);
        
        Assert.Equal(validFiles.Count, result.Count);
        Assert.True(result.Select(x => x.Id).SequenceEqual(validFiles.Select(x => x.Id)));
    }*/
}