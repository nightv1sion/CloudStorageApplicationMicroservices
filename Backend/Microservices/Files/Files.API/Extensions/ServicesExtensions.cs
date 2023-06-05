using DatabaseInfrastructure.Helper;
using Files.API.Model;
using Files.API.Services;
using Files.API.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Files.API.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration["ASPNETCORE_ENVIRONMENT"] == "Development")
        {
            services.AddDbContext<ApplicationDatabaseContext>(
                x =>
                {
                    x.UseInMemoryDatabase("TestDatabase");
                });
        }
        else {
            services.AddDbContext<ApplicationDatabaseContext>(
            x => x.UseSqlServer(
                configuration.GetConnectionString()));
        }
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
    }
}