using Audio.API.Model;
using DatabaseInfrastructure.Helper;
using Microsoft.EntityFrameworkCore;

namespace Audio.API.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDatabaseContext>(
            x => x.UseSqlServer(
                ConnectionStringHelper.GetConnectionString(configuration)));
    }
}