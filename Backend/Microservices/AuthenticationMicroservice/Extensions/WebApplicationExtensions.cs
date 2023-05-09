using AuthenticationMicroservice.Model;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationMicroservice.Extensions;

public static class WebApplicationExtensions
{
    public static void MigrateDatabase(this WebApplication app)
    {
        var context = app.Services.GetRequiredService<ApplicationDatabaseContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
}