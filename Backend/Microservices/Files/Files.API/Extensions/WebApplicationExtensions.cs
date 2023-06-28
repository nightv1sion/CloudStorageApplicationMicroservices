using Files.API.Model.Database;
using Microsoft.EntityFrameworkCore;

namespace Files.API.Extensions;
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