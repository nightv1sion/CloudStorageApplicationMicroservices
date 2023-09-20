using Files.Application.Features.File.DataTransferObjects;
using FluentValidation;

namespace Files.Application.Features.File.Validators;

public class UpdateFileDtoValidator : AbstractValidator<UpdateFileDto>
{
    public UpdateFileDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }   
}