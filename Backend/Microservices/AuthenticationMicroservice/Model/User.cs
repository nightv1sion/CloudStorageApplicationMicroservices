using Microsoft.AspNetCore.Identity;

namespace AuthenticationMicroservice.Model;

public class User : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}