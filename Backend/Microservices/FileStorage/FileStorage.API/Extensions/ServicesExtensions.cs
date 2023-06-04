using DatabaseInfrastructure.Helper;
using FileStorage.API.Model;
using FileStorage.API.Services;
using FileStorage.API.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.API.Extensions;

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
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IFileService, FileService>();
    }
}