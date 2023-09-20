using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace Files.Application.Features.File.DataTransferObjects;

public class FormFileDto
{
    public IFormFile? File { get; set; }
    public string Name { get; set; }
    public string FileExtension { get; set; }
    public Guid? DirectoryId { get; set; }
    public static ValueTask<FormFileDto> BindAsync(HttpContext context, ParameterInfo parameterInfo)
    {
        var directoryIdFormValue = context.Request.Form["DirectoryId"].FirstOrDefault();
        Guid? directoryId = String.IsNullOrEmpty(directoryIdFormValue) ? null : Guid.Parse(directoryIdFormValue);
        var file = context.Request.Form.Files.GetFile("File");
        if (file is null)
        {
            return ValueTask.FromResult(new FormFileDto());
        }
        var name = Path.GetFileNameWithoutExtension(file.FileName);
        var extension = Path.GetExtension(file.FileName);
        return ValueTask.FromResult(new FormFileDto()
        {
            File = file,
            Name = name,
            FileExtension = extension,
            DirectoryId = directoryId
        });
    }
}