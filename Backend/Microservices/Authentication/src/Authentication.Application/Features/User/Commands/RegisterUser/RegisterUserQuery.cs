using Authentication.Application.Features.User.DataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Application.Features.User.Commands.RegisterUser;
public class RegisterUserQuery : IRequest<Dictionary<string, string[]>>
{
    public RegisterUserQuery(RegisterUserDto dto)
    {
        Dto = dto;
    }
    public RegisterUserDto Dto { get; }

}