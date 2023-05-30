namespace Audio.API.Model;

public class AudioFile
{
    public Guid Id { get; set; }
    public string FileSystemName { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public Guid UserId { get; set; }
}