using Files.Application.Features.Directory.DataTransferObjects;
using FluentValidation;

namespace Files.Application.Features.Directory.Validators;

public class CreateDirectoryDtoValidator : AbstractValidator<CreateDirectoryDto>
{
    public CreateDirectoryDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Directories).NotNull();
        RuleFor(x => x.Files).NotNull();
    }
}