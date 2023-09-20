using Microsoft.Extensions.Configuration;

namespace DatabaseInfrastructure.Helper;

public static class ConnectionStringHelper
{
    public static string GetConnectionString(this IConfiguration configuration)
    {
        return configuration["ConnectionStrings:PostgresDatabase"]!;
    }
}