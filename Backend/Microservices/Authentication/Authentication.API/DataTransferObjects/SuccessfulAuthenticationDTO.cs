using System.IdentityModel.Tokens.Jwt;

namespace Authentication.API.DataTransferObjects;

public class SuccessfulAuthenticationDTO
{
    public JwtSecurityToken Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ValidTo { get; set; }
}