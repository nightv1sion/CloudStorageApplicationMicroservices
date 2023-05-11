using AuthenticationMicroservice.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationMicroservice.Services;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUserAsync(RegisterUserDTO dto);
    Task<SuccessfulAuthenticationDTO> LoginUserAsync(LoginUserDTO dto);
}