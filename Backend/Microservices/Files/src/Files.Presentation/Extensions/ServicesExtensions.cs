using System.Reflection;
using DatabaseInfrastructure.Helper;
using Files.Application.Common.Behaviours;
using Files.Application.Extensions.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Files.Application.Extensions.Services;
using Files.Infrastructure.Messaging.Consumers;
using Files.Infrastructure.Persistence;
using Files.Infrastructure.Persistence.RepositoryManagers;
using MediatR;

namespace Files.Presentation.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDatabaseContext>(
            (x) =>
            {
                x.UseNpgsql(configuration.GetConnectionString());
            });
    }

    public static void ConfigureMediator(this IServiceCollection services)
    {
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(
                Assembly.GetAssembly(typeof(Application.Common.Exceptions.InvalidFileBadRequestException))!);
        });
    }
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IDirectoryService, DirectoryService>();
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    }

    public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<FileCreatedFaultConsumer>();
            x.AddConsumer<FileSavedConsumer>();
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["RabbitMQ:Host"], configuration["RabbitMQ:VirtualHost"], c =>
                {
                    c.Username(configuration["RabbitMQ:Username"]);
                    c.Password(configuration["RabbitMQ:Password"]);
                });
                
                config.ConfigureEndpoints(context);
            });
        });
    }
}