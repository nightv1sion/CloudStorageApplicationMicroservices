using Files.Application.Features.Directory.DataTransferObjects;
using FluentValidation;

namespace Files.Application.Features.Directory.Validators;

public class UpdateDirectoryDtoValidator : AbstractValidator<UpdateDirectoryDto>
{
    public UpdateDirectoryDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Files).NotEmpty();
        RuleFor(x => x.Directories).NotEmpty();
    }
}