using DatabaseInfrastructure.Helper;
using Files.API.Model;
using Files.API.Services;
using Files.API.Services.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Models.File;

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

    public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            //x.AddRequestClient<RetrieveFile>();
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["RABBIT_MQ_HOSTNAME"], configuration["RABBIT_MQ_VIRTUAL_HOST"], c =>
                {
                    c.Username(configuration["RABBIT_MQ_USERNAME"]);
                    c.Password(configuration["RABBIT_MQ_PASSWORD"]);
                });
                
                config.ConfigureEndpoints(context);
            });
        });
    }
}