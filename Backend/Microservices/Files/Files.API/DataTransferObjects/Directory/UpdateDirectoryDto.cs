namespace Files.API.DataTransferObjects.Directory;

public class UpdateDirectoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentDirectoryId { get; set; }
    public ICollection<Guid> Directories { get; set; }
    public ICollection<Guid> Files { get; set; }
}