using AudioFilesMicroservice.Model;
using DatabaseInfrastructure.Helper;
using Microsoft.EntityFrameworkCore;

namespace AudioFilesMicroservice.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDatabaseContext>(
            x => x.UseSqlServer(
                ConnectionStringHelper.GetConnectionString(configuration)));
    }
}