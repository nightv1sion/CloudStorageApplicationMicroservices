using System.Reflection;

namespace Audio.API.DataTransferObjects;

public class AudioFileDTO
{
    public IFormFile? File { get; set; }
    public string Name { get; set; }
    public string FileExtension { get; set; }
    public string FileSystemName { get; set; }
    public static ValueTask<AudioFileDTO> BindAsync(HttpContext context, ParameterInfo parameterInfo)
    {
        var file = context.Request.Form.Files.GetFile("File");
        var name = context.Request.Form["Name"];
        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        var extension = Path.GetExtension(file.FileName);
        return ValueTask.FromResult(new AudioFileDTO()
        {
            File = file,
            Name = name,
            FileSystemName = fileName,
            FileExtension = extension
        });
    }
}