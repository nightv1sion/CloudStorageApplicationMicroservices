using Microsoft.Extensions.DependencyInjection;

namespace Routes.Helpers;

public static class Extensions
{
    public static void AddRoutePatternHelper(this IServiceCollection services)
    {
        services.AddScoped<IRoutePatternHelper, RoutePatternHelper>();
    }
}