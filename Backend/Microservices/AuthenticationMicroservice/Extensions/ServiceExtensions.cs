using AuthenticationMicroservice.Model;
using AuthenticationMicroservice.Services;
using AuthenticationMicroservice.Services.Contracts;
using DatabaseInfrastructure.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationMicroservice.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDatabaseContext>(options =>
            options.UseSqlServer(ConnectionStringHelper.GetConnectionString(configuration)));
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredUniqueChars = 1;
        }).AddEntityFrameworkStores<ApplicationDatabaseContext>();
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }
}