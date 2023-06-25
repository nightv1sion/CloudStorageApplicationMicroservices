namespace Files.API.DataTransferObjects;

public class CreateDirectoryDto
{
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
}