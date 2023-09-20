using Authentication.Application.Features.User.DataTransferObjects;
using MediatR;

namespace Authentication.Application.Features.User.Commands.LoginUser;

public class LoginUserQuery : IRequest<SuccessfulAuthenticationDto>
{
    public LoginUserQuery(LoginUserDto dto)
    {
        Dto = dto;
    }
    public LoginUserDto Dto { get; }

}