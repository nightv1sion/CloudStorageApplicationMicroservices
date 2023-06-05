namespace Files.API.Model;

public class File
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public Guid UserId { get; set; }
}