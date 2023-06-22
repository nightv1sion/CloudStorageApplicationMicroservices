using AutoMapper;
using Files.API.Exceptions;
using Files.API.Model;
using Files.API.Services;
using Files.API.Services.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.File;
using Moq;
using Moq.EntityFrameworkCore;
using File = Files.API.Model.File;

namespace Files.API.UnitTests.Services;

public class FileServiceTests
{
    private readonly IFileService _service;
    private readonly Mock<ApplicationDatabaseContext> _context;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IPublishEndpoint> _publishEndpoint;
    private readonly Mock<IRequestClient<RetrieveFile>> _client;
    private readonly Mock<ILogger<FileService>> _logger;

    public FileServiceTests()
    {
        _context = new Mock<ApplicationDatabaseContext>(
            () => new ApplicationDatabaseContext(new DbContextOptions<ApplicationDatabaseContext>()));
        _mapper = new Mock<IMapper>();
        _publishEndpoint = new Mock<IPublishEndpoint>();
        _client = new Mock<IRequestClient<RetrieveFile>>();
        _logger = new Mock<ILogger<FileService>>();
        _service = new FileService(
            _context.Object, _mapper.Object, _publishEndpoint.Object, _client.Object, _logger.Object);
    }

    [Fact]
    public async Task GetFile_NotExistingFileId_ThrowsInvalidFileIdBadRequest()
    {
        _context.Setup<DbSet<File>>(expression: x =>
            x.Files)
            .ReturnsDbSet(new List<File>());

        var func = async () => await _service.GetFileAsync(Guid.NewGuid(), Guid.NewGuid());

        await Assert.ThrowsAsync<InvalidFileIdBadRequest>(func);
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
        
        Assert.Equal(file, result);
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

        var result = await _service.GetFilesByUserIdAsync(userId);
        
        Assert.Equal(validFiles.Count, result.Count);
        Assert.True(result.SequenceEqual(validFiles));
    }
}