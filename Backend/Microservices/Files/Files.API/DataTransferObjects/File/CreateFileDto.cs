namespace Files.API.DataTransferObjects.File;

public class CreateFileDto
{
    public string Name { get; set; }
    public string Extension { get; set; }
    public Guid UserId { get; set; }
}