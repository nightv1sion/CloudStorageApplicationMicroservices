using Authentication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Middlewares.ExceptionHandling;
using Serilog;

namespace Authentication.Presentation.Extensions;

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

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
    public static void MigrateDatabase(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
}