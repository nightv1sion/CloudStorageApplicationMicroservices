using Files.Application.Features.File.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.File.Commands.CreateFile;

public class UploadFileCommand : IRequest<FileDto>
{
    public UploadFileCommand(
        Guid userId,
        FormFileDto dto)
    {
        UserId = userId;
        Dto = dto;
    }

    public Guid UserId { get; }
    public FormFileDto Dto { get; }
}