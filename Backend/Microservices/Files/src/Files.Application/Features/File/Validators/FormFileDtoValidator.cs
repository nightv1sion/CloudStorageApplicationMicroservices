using Files.Application.Features.File.DataTransferObjects;
using FluentValidation;

namespace Files.Application.Features.File.Validators;

public class FormFileDtoValidator : AbstractValidator<FormFileDto>
{
    public FormFileDtoValidator()
    {
        RuleFor(x => x.File).NotNull();
    }
}