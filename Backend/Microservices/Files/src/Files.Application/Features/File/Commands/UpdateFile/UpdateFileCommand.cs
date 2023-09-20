using Files.Application.Features.File.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.File.Commands.UpdateFile;

public class UpdateFileCommand : IRequest<FileDto>
{
    public UpdateFileCommand(
        Guid userId,
        UpdateFileDto dto)
    {
        UserId = userId;
        Dto = dto;
    }

    public Guid UserId { get; }
    public UpdateFileDto Dto { get; }
}