namespace Files.API.DataTransferObjects.Directory;

public class CreateDirectoryDto
{
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public ICollection<Guid> Directories { get; set; }
    public ICollection<Guid> Files { get; set; }
}