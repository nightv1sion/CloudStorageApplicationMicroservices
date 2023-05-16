using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Authentication.API.DataTransferObjects;
using Authentication.API.Exceptions.BadRequest;
using Authentication.API.Exceptions.Unauthorized;
using Authentication.API.Model;
using Microsoft.AspNetCore.Identity;
using Middlewares.ExceptionHandling.Exceptions;

namespace Authentication.API.Services.Contracts;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        UserManager<User> userManager,
        ITokenService tokenService,
        IConfiguration configuration,
        ILogger<AuthenticationService> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _configuration = configuration;
        _logger = logger;
        _logger.LogInformation("Authentication Service called");
    }
    public async Task<IdentityResult> RegisterUserAsync(RegisterUserDTO dto)
    {
        var user = new User()
        {
            UserName = dto.Username,
            Email = dto.EmailAddress,
        };
        var result = await _userManager.CreateAsync(user, dto.Password);

        _logger.LogInformation(
            $"User with username '{user.UserName}' successfully registered: {result.Succeeded}");
        return result;
    }

    public async Task<SuccessfulAuthenticationDTO> LoginUserAsync(LoginUserDTO dto)
    {
        var user = await _userManager.FindByNameAsync(dto.Username);
        if (user == null)
        {
            throw new InvalidUserNameUnauthorized(dto.Username);
        }

        var checkUserPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (checkUserPassword == false)
        {
            throw new InvalidUserPasswordUnauthorized(dto.Username);
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
        
        _logger.LogInformation($"User '{user.UserName} successfully logged in'");
        return result;
    }

    public async Task<TokenDTO> GetRefreshTokenAsync(TokenDTO tokenDto)
    {
        string accessToken = tokenDto.AccessToken;
        string refreshToken = tokenDto.RefreshToken;

        var principal = _tokenService.GetPrincipaFromExpiredToken(accessToken);
        var username = principal.Identity?.Name;
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new InvalidAccessTokenBadRequestException(username);
        }

        if (user.RefreshToken != refreshToken)
        {
            throw new InvalidRefreshTokenBadRequestException(username);
        }

        if (user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new RefreshTokenIsExpiredBadRequest(username);
        }

        var newAccessToken = _tokenService.CreateToken(principal.Claims.ToList());
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        var newTokenDto = new TokenDTO()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken
        };
       
        _logger.LogInformation($"User '{user.UserName}': refresh token successfully generated");
        return newTokenDto;
    }
}