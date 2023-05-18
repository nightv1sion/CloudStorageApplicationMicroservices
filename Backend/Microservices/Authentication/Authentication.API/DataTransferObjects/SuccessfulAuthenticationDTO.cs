using System.IdentityModel.Tokens.Jwt;

namespace Authentication.API.DataTransferObjects;

public class SuccessfulAuthenticationDTO
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ValidTo { get; set; }
}