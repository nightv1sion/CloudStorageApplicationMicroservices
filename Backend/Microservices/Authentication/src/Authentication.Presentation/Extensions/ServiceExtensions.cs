using System.Reflection;
using Authentication.Application.Common.Behaviours;
using Authentication.Application.Extensions.Services;
using Authentication.Application.Extensions.Services.Contracts;
using Authentication.Domain.Entities;
using Authentication.Infrastructure.Persistence;
using DatabaseInfrastructure.Helper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Presentation.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDatabaseContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString()));
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

    public static void ConfigureMediator(this IServiceCollection services)
    {
        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssembly(
                Assembly.GetAssembly(typeof(Application.Common.Exceptions.Unauthorized.InvalidUserNameUnauthorized))!);
        });
        
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }
}