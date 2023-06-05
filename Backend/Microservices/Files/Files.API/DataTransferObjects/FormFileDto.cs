using System.Reflection;

namespace Files.API.DataTransferObjects;

public class FormFileDto
{
    public IFormFile? File { get; set; }
    public string Name { get; set; }
    public string FileExtension { get; set; }
    public static ValueTask<FormFileDto> BindAsync(HttpContext context, ParameterInfo parameterInfo)
    {
        var file = context.Request.Form.Files.GetFile("File");
        var name = Path.GetFileNameWithoutExtension(file.FileName);
        var extension = Path.GetExtension(file.FileName);
        return ValueTask.FromResult(new FormFileDto()
        {
            File = file,
            Name = name,
            FileExtension = extension
        });
    }
}