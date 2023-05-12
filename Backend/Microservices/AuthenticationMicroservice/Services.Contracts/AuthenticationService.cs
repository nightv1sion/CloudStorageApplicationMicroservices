using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthenticationMicroservice.DataTransferObjects;
using AuthenticationMicroservice.Exceptions;
using AuthenticationMicroservice.Model;
using Microsoft.AspNetCore.Identity;
using Middlewares.ExceptionHandling.Exceptions;

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
            throw new UnauthorizedException();
        }

        var checkUserPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (checkUserPassword == false)
        {
            throw new UnauthorizedException();
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

    public async Task<TokenDTO> GetRefreshTokenAsync(TokenDTO tokenDto)
    {
        string accessToken = tokenDto.AccessToken;
        string refreshToken = tokenDto.RefreshToken;

        var principal = _tokenService.GetPrincipaFromExpiredToken(accessToken);
        var username = principal.Identity?.Name;
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new InvalidAccessTokenBadRequestException();
        }

        if (user.RefreshToken != refreshToken)
        {
            throw new InvalidRefreshTokenBadRequestException();
        }

        if (user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new RefreshTokenIsExpiredBadRequest();
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

        return newTokenDto;
    }
}