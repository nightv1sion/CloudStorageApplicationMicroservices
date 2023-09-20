using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authentication.Application.Extensions.Services.Contracts;

public interface ITokenService
{
    JwtSecurityToken CreateToken(List<Claim> authClaims);
    string GenerateRefreshToken();
    (ClaimsPrincipal claimsPrincipal, DateTime validTo) GetPrincipalFromAccessToken(string token);
}