using System.Reflection;

namespace FileStorage.API.DataTransferObjects;

public class FileDto
{
    public IFormFile? File { get; set; }
    public string Name { get; set; }
    public string FileExtension { get; set; }
    public string FileSystemName { get; set; }
    public static ValueTask<FileDto> BindAsync(HttpContext context, ParameterInfo parameterInfo)
    {
        var file = context.Request.Form.Files.GetFile("File");
        var name = context.Request.Form["Name"];
        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        var extension = Path.GetExtension(file.FileName);
        return ValueTask.FromResult(new FileDto()
        {
            File = file,
            Name = name,
            FileSystemName = fileName,
            FileExtension = extension
        });
    }
}