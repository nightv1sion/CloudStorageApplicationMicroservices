using System.Security.Claims;
using AuthenticationMicroservice.DataTransferObjects;
using AuthenticationMicroservice.Model;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationMicroservice.Services.Contracts;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthenticationService(
        UserManager<User> userManager,
        ITokenService tokenService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _configuration = configuration;
    }
    public async Task<IdentityResult> RegisterUserAsync(RegisterUserDTO dto)
    {
        var user = new User()
        {
            UserName = dto.Username,
            Email = dto.EmailAddress,
        };
        var result = await _userManager.CreateAsync(user, dto.Password);

        return result;
    }

    public async Task<SuccessfulAuthenticationDTO> LoginUserAsync(LoginUserDTO dto)
    {
        var user = await _userManager.FindByNameAsync(dto.Username);
        if (user == null)
        {
            return null;
        }

        var checkUserPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (checkUserPassword == false)
        {
            return null ;
        }

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
        };

        var token = _tokenService.CreateToken(authClaims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        if (!int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays))
        {
            throw new InvalidOperationException("Environment variable JWT:TokenValidityInMinutes does not exists");
        }

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

        await _userManager.UpdateAsync(user);

        var result = new SuccessfulAuthenticationDTO()
        {
            Token = token,
            RefreshToken = refreshToken,
            ValidTo = token.ValidTo
        };
        return result;
    }
}