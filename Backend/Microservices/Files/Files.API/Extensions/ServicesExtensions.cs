using DatabaseInfrastructure.Helper;
using Files.API.Model;
using Files.API.Services;
using Files.API.Services.Contracts;
using MassTransit;
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

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFileService, FileService>();

        services.AddMassTransit(x =>
        {
            x.AddMassTransit(provider =>
                provider.UsingRabbitMq((context, config) =>
                {
                    config.Host(new Uri(configuration["RABBIT_MQ_USERNAME"]), "/", c =>
                    {
                        c.Username(configuration["RABBIT_MQ_USERNAME"]);
                        c.Password(configuration["RABBIT_MQ_PASSWORD"]);
                    });
                }));
            /*Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(new Uri(configuration["RABBIT_MQ_PATH"]), "/", h =>
                    {
                        h.Username(configuration["RABBIT_MQ_USERNAME"]);
                        h.Password(configuration["RABBIT_MQ_PASSWORD"]);
                    });
                }));*/
        });
    }
}