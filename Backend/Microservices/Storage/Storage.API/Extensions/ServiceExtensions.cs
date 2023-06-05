using Storage.API.Services;
using Storage.API.Services.Contracts;

namespace Storage.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, StorageService>();
    }
}