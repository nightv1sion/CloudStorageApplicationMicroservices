using Authentication.Application.Features.User.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Application.Extensions.Services.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUserAsync(RegisterUserDto dto);
    Task<SuccessfulAuthenticationDto> LoginUserAsync(LoginUserDto dto);
    Task<TokenDto> GetRefreshTokenAsync(TokenDto tokenDto);
    (bool isSuccessful , Guid userId) ValidateUser(string accessToken);
}