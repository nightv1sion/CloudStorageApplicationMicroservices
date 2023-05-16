using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Model;

public class User : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}