namespace Files.Application.Features.File.DataTransferObjects;

public class UpdateFileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? DirectoryId { get; set; }
}