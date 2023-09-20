using Microsoft.EntityFrameworkCore;
using Files.Infrastructure.Persistence;
using Middlewares.Authentication;
using Middlewares.ExceptionHandling;
using Serilog;

namespace Files.Presentation.Extensions;
public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        app.MigrateDatabase();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<JwtAuthenticationMiddleware>();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
    private static void MigrateDatabase(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetService<ApplicationDatabaseContext>();
        if (context is not null && context.Database.IsRelational() && context.Database.GetPendingMigrations().Any() )
        {
            context.Database.Migrate();
        }
    }
}