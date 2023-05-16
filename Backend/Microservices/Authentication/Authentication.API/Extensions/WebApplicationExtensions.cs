using Authentication.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Extensions;

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