using Files.Domain.Enums;

namespace Files.Application.Features.File.DataTransferObjects;

public class FileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public Guid UserId { get; set; }
    public UploadingStatus UploadingStatus { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public Guid? DirectoryId { get; set; }
}