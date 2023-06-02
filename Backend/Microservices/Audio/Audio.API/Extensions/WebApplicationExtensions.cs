using Audio.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Audio.API.Extensions;

public static class WebApplicationExtensions
{
    public static void MigrateDatabase(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetService<ApplicationDatabaseContext>();
        if (context.Database.IsRelational() && context.Database.GetPendingMigrations().Any() )
        {
            context.Database.Migrate();
        }
    }
}