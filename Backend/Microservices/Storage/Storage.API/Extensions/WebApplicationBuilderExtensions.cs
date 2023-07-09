using Serilog;

namespace Storage.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration)
            => configuration.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureServices();
        builder.Services.ConfigureMassTransit(builder.Configuration);
        
        return builder;
    }
}