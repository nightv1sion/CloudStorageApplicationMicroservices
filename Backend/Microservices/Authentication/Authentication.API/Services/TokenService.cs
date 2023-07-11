using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Authentication.API.Exceptions.BadRequest;
using Authentication.API.Services.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.API.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TokenService> _logger;

    public TokenService(
        IConfiguration configuration,
        ILogger<TokenService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _logger.LogInformation("Token Service was called");
    }
    public JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var jwtSecret = _configuration["JWT_SECRET"];
        if (string.IsNullOrEmpty(jwtSecret))
        {
            throw new InvalidOperationException("Environment variable JWT_SECRET does not exists");
        }
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        if (!int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes))
        {
            throw new InvalidOperationException("Environment variable JWT:TokenValidityInMinutes does not exists");
        }

        var expiredDateTime = DateTime.Now.AddMinutes(tokenValidityInMinutes);
        var signingCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(expires: expiredDateTime, claims: authClaims,
            signingCredentials: signingCredentials);
        
        _logger.LogInformation("Token was successfully created");
        return token;    
;    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public (ClaimsPrincipal claimsPrincipal, DateTime validTo) GetPrincipalFromAccessToken(string token)
    {
        var jwtSecret = _configuration["JWT_SECRET"];
        if (string.IsNullOrEmpty(jwtSecret))
        {
            throw new InvalidOperationException("Environment variable JWT_SECRET does not exists");
        }
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        ClaimsPrincipal principal;
        SecurityToken securityToken;
        try
        {
            principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                out SecurityToken securityTokenRef);
            securityToken = securityTokenRef;
        }
        catch (Exception ex)
        {
            throw new InvalidAccessTokenBadRequestException();
        }
        
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        
        _logger.LogInformation("Successfully retrieved principal from expired access token");
        return (principal, securityToken.ValidTo);
    }
}