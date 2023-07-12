using Authentication.Application.Extensions.Services.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Application.Features.User.Commands.RegisterUser;

public class RegisterUserQueryHandler : IRequestHandler<RegisterUserQuery, Dictionary<string, string[]>>
{
    private readonly IAuthenticationService _authenticationService;

    public RegisterUserQueryHandler(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    public async Task<Dictionary<string, string[]>> Handle(
        RegisterUserQuery request, 
        CancellationToken cancellationToken)
    {
        var result = await _authenticationService.RegisterUserAsync(request.Dto);
        var errors = result.Errors.GroupBy(x => x.Code)
            .ToDictionary(x => x.Key, y => y.Select(
                element => element.Description).ToArray());
        return errors;
    }
}