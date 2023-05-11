using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationMicroservice.DataTransferObjects;

public class SuccessfulAuthenticationDTO
{
    public JwtSecurityToken Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ValidTo { get; set; }
}