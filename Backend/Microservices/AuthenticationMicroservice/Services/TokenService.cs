using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthenticationMicroservice.Services.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationMicroservice.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_SECRET"]));
        if (!int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes))
        {
            throw new InvalidOperationException("Environment variable JWT:TokenValidityInMinutes does not exists");
        }

        var expiredDateTime = DateTime.Now.AddMinutes(tokenValidityInMinutes);
        var signingCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(expires: expiredDateTime, claims: authClaims,
            signingCredentials: signingCredentials);

        return token;    
;    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipaFromExpiredToken(string token)
    {
        var jwtSecret = _configuration["JWT_SECRET"];
        if (jwtSecret == null)
        {
            throw new InvalidOperationException("Environment variable JWT_SECRET does not exists");
        }
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}