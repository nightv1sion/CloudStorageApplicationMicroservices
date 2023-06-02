using System.Reflection;

namespace Audio.API.DataTransferObjects;

public class AudioFileDTO
{
    public IFormFile? File { get; set; }

    public static ValueTask<AudioFileDTO> BindAsync(HttpContext context, ParameterInfo parameterInfo)
    {
        var file = context.Request.Form.Files.GetFile("File");
        return ValueTask.FromResult(new AudioFileDTO()
        {
            File = file
        });
    }
}