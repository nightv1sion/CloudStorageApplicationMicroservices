using Authentication.API.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Services.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUserAsync(RegisterUserDTO dto);
    Task<SuccessfulAuthenticationDTO> LoginUserAsync(LoginUserDTO dto);
    Task<TokenDTO> GetRefreshTokenAsync(TokenDTO tokenDto);
    bool ValidateUser(string accessToken);

}