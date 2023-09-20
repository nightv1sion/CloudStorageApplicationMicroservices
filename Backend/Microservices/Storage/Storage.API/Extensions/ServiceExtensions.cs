using MassTransit;
using MassTransit.Configuration;
using Storage.API.MassTransit.Consumers;
using Storage.API.Services;
using Storage.API.Services.Contracts;

namespace Storage.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IFileSystemService, FileSystemService>();
    }

    public static void ConfigureMassTransit(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<FileCreatedConsumer>();
            x.AddConsumer<FileDeletedConsumer>();
            x.AddConsumer<RetrieveFileConsumer>();
            x.UsingRabbitMq(
                (context, config) =>
                {
                    config.Host(
                        configuration["RabbitMQ:Host"],
                        configuration["RabbitMQ:VirtualHost"], c =>
                        {
                            c.Username(configuration["RabbitMQ:Username"]);
                            c.Password(configuration["RabbitMQ:Password"]);
                        });
                    config.ReceiveEndpoint("file-created-event",
                        endpoint =>
                        {
                            endpoint.UseMessageRetry(retryConfig =>
                            {
                                retryConfig.SetRetryPolicy(
                                    policy=> policy.Interval(5, TimeSpan.FromSeconds(30)));
                            });
                            endpoint.ConfigureConsumer<FileCreatedConsumer>(context);
                        });
                    config.ReceiveEndpoint("file-deleted-event",
                        endpoint =>
                        {
                            endpoint.UseMessageRetry(retryConfig =>
                            {
                                retryConfig.SetRetryPolicy(
                                    policy=> policy.Interval(5, TimeSpan.FromSeconds(30)));
                            });
                            endpoint.ConfigureConsumer<FileDeletedConsumer>(context);
                        });
                    config.ReceiveEndpoint("retrieve-file-command",
                        endpoint =>
                        {
                            endpoint.ConfigureConsumer<RetrieveFileConsumer>(context);
                        });
                });
        });
    }
}