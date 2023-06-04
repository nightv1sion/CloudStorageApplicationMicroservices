using Microsoft.Extensions.Configuration;

namespace DatabaseInfrastructure.Helper;

public static class ConnectionStringHelper
{
    public static string GetConnectionString(this IConfiguration configuration)
    {
        return
            $"Data Source={configuration["DATABASE_HOST"]},{configuration["DATABASE_PORT"]}; Initial Catalog={configuration["DATABASE_NAME"]}; User ID=sa; Password={configuration["DATABASE_PASSWORD"]}; TrustServerCertificate=true; Encrypt=false;";
    }
}