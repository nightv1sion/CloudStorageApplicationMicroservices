using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Entities;

public class User : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}