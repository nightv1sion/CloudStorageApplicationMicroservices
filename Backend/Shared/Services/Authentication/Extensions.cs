using Microsoft.Extensions.DependencyInjection;

namespace Services.Authentication;

public static class Extensions
{
    public static void ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }
}