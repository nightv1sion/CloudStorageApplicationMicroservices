using System.Reflection;
using FluentValidation;
using Serilog;

namespace Authentication.Presentation.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) 
            => configuration.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureDatabaseContext(builder.Configuration);
        builder.Services.ConfigureIdentity();
        builder.Services.AddValidatorsFromAssembly(
            Assembly.GetAssembly(typeof(Application.Common.Exceptions.Unauthorized.InvalidUserNameUnauthorized)));
        builder.Services.ConfigureServices();
        builder.Services.ConfigureMediator();

        return builder;
    }
}