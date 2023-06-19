using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Storage.API.Exceptions;
using Storage.API.Services;
using Storage.API.Services.Contracts;

namespace Storage.API.UnitTests.Services;

public class StorageServiceTests
{
    private readonly Mock<IFileSystemService> _fileSystemServiceMock;
    private readonly IStorageService _storageService;
    private readonly Mock<ILogger<StorageService>> _storageServiceLoggerMock;

    public StorageServiceTests()
    {
        _fileSystemServiceMock = new Mock<IFileSystemService>();
        _storageServiceLoggerMock = new Mock<ILogger<StorageService>>();
        Mock<IConfiguration> configuration = new();
        configuration
            .SetupGet(x => x[It.IsAny<string>()])
            .Returns(Guid.NewGuid().ToString());
        
        _storageService = new StorageService(
            configuration.Object, 
            _fileSystemServiceMock.Object,
            _storageServiceLoggerMock.Object);
    }

    [Fact]
    public void StorageService_GetFileBytesAsync_ThrowsInvalidFileNameBadRequestException()
    {
        _fileSystemServiceMock.Setup(
                x => x.CheckIfFileExists(It.IsAny<string>()))
            .Returns(true);
        var fileName = new Guid().ToString();
        
        var func = async () => await _storageService.GetFileBytesAsync(fileName);

        Assert.ThrowsAsync<InvalidFileNameBadRequestException>(func);
    }
    
    [Fact]
    public void StorageService_SaveFileBytesAsync_ThrowsInvalidFileNameBadRequestException()
    {
        _fileSystemServiceMock.Setup(
                x => x.CheckIfFileExists(It.IsAny<string>()))
            .Returns(true);
        
        var fileName = new Guid().ToString();
        var fileExtension = new Guid().ToString();
        var bytes = Array.Empty<byte>();

        var func = async () => await _storageService.SaveFileBytesAsync(bytes, fileName, fileExtension);

        Assert.ThrowsAsync<FileExistsBadRequestException>(func);
    }
}