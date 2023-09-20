using Files.Domain.Enums;

namespace Files.Domain.Entities.File;

public class File
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public Guid UserId { get; set; }
    public UploadingStatus UploadingStatus { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public Files.Domain.Entities.Directory.Directory Directory { get; set; }
    public Guid? DirectoryId { get; set; }
}