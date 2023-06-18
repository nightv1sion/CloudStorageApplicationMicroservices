using MassTransit;
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
                        configuration["RABBIT_MQ_HOSTNAME"],
                        configuration["RABBIT_MQ_VIRTUAL_HOST"], c =>
                        {
                            c.Username(configuration["RABBIT_MQ_USERNAME"]);
                            c.Password(configuration["RABBIT_MQ_PASSWORD"]);
                        });
                    config.ReceiveEndpoint("file-created-event",
                        endpoint =>
                        {
                            endpoint.ConfigureConsumer<FileCreatedConsumer>(context);
                        });
                    config.ReceiveEndpoint("file-deleted-event",
                        endpoint =>
                        {
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