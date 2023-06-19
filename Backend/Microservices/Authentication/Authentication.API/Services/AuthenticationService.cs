using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Authentication.API.DataTransferObjects;
using Authentication.API.Exceptions.BadRequest;
using Authentication.API.Exceptions.Unauthorized;
using Authentication.API.Model;
using Authentication.API.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Services;

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
        _logger.LogInformation("Authentication Service was called");
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
            $"User with username '{user.UserName}' was successfully registered: {result.Succeeded}");
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
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Expired, user.UserName),
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
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var result = new SuccessfulAuthenticationDTO()
        {
            Token = tokenHandler.WriteToken(token),
            RefreshToken = refreshToken,
            ValidTo = token.ValidTo
        };
        
        _logger.LogInformation($"User '{user.UserName} was successfully logged in'");
        return result;
    }

    public async Task<TokenDTO> GetRefreshTokenAsync(TokenDTO tokenDto)
    {
        string accessToken = tokenDto.AccessToken;
        string refreshToken = tokenDto.RefreshToken;

        var (principal, _) = _tokenService.GetPrincipalFromAccessToken(accessToken);
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
       
        _logger.LogInformation($"User '{user.UserName}': was refresh token successfully generated");
        return newTokenDto;
    }

    public (bool isSuccessful, Guid userId) ValidateUser(string accessToken)
    {
        var (claims, validTo) = _tokenService.GetPrincipalFromAccessToken(accessToken);
        if (validTo < DateTime.Now)
        {
            throw new InvalidAccessTokenBadRequestException();
        }

        var idClaim = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (idClaim == null)
        {
            throw new InvalidAccessTokenBadRequestException();
        }

        if (Guid.TryParse(idClaim, out Guid userId))
        {
            return (true, userId);
        }

        throw new InvalidAccessTokenBadRequestException();
    }
}