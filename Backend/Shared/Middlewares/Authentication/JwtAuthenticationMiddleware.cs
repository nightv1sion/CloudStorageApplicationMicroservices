using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Middlewares.Authentication;

public class JwtAuthenticationMiddleware 
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public JwtAuthenticationMiddleware(
        RequestDelegate next,
        IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
        if(token is not null)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSecret = _configuration["JWT_SECRET"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret!));
                var result = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = key,
                });
                var userIdClaim = result.Claims.FirstOrDefault(x => x.Key == ClaimTypes.NameIdentifier).Value;
                if (userIdClaim is not null)
                {
                    var userId = Guid.Parse(userIdClaim.ToString()!);
                    context.Items["UserId"] = userId;
                    await _next(context);
                    return;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }
}