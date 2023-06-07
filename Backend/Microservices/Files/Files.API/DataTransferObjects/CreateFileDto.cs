namespace Files.API.DataTransferObjects;

public class CreateFileDto
{
    public string Name { get; set; }
    public string Extension { get; set; }
    public Guid UserId { get; set; }
}