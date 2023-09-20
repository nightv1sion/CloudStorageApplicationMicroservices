using AutoMapper;
using Files.Application.Common.Exceptions;
using Files.Application.Extensions.Interfaces;
using Files.Application.Features.File;
using Files.Application.Extensions.Services;
using Files.Infrastructure.Persistence.RepositoryManagers;
using MassTransit;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Models.File;
using Moq;
using Directory = Files.Domain.Entities.Directory.Directory;
using File = Files.Domain.Entities.File.File;
namespace Files.Application.UnitTests.Services;

public class FileServiceTests
{
    private readonly IFileService _service;
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
        _repositoryManager.Setup(x => x.DirectoryRepository.FindAll(It.IsAny<bool>()))
            .Returns(Enumerable.Empty<Directory>().AsQueryable().BuildMock());

        var func = async () => await _service.GetFileAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        await Assert.ThrowsAsync<InvalidDirectoryIdBadRequestException>(func);
    }
    [Fact]
    public async Task GetFile_ExistingFileId_ReturnsValidFile()
    {
        var userId = Guid.NewGuid();
        var directoryId = Guid.NewGuid();
        var fileId = Guid.NewGuid();
        var file = new File()
        {
            Id = fileId,
            UserId = userId,
            DirectoryId = directoryId
        };
        
        var directoriesList = new List<Directory>()
        {
            new()
            {
                Id = directoryId,
                UserId = userId,
                Files = new List<File>{file}
            }
        };


        _repositoryManager.Setup(x => x.DirectoryRepository.FindAll(It.IsAny<bool>()))
            .Returns(directoriesList.AsQueryable().BuildMock());

        var result = await _service.GetFileAsync(userId, directoryId, fileId);
        
        Assert.Equal(file.Id, result.Id);
    }
}