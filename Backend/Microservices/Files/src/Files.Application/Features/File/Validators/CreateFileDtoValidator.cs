using Files.Application.Features.File.DataTransferObjects;
using FluentValidation;

namespace Files.Application.Features.File.Validators;

public class CreateFileDtoValidator : AbstractValidator<CreateFileDto>
{
    public CreateFileDtoValidator()
    {
        RuleFor(x => x.Extension).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}