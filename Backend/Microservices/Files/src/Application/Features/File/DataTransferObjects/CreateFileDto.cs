namespace Files.Application.Features.File.DataTransferObjects;

public class CreateFileDto
{
    public string Name { get; set; }
    public string Extension { get; set; }
    public Guid UserId { get; set; }
    public Guid? DirectoryId { get; set; }
}