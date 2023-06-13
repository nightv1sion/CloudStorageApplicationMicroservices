using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace Routes.Helpers;

public class RoutePatternHelper : IRoutePatternHelper
{
    public RouteValueDictionary GetRouteValues(string routePattern, string route)
    {
        var matcher = new TemplateMatcher(TemplateParser.Parse(routePattern), new RouteValueDictionary());

        var routeValues = new RouteValueDictionary();

        bool isMatch = matcher.TryMatch(route, routeValues);

        return routeValues;
    }
    public string SetRouteValuesIntoPattern(
        string routePattern, RouteValueDictionary routeValues)
    {
        var matcher = new TemplateMatcher(TemplateParser.Parse(routePattern), routeValues);
        var path = string.Empty;

        foreach (var segment in matcher.Template.Segments)
        {
            var pathSegment = segment.Parts.FirstOrDefault();
            if (pathSegment is { Text: null })
            {
                if (pathSegment.Name != null && matcher.Defaults.TryGetValue(pathSegment.Name, out object? result))
                {
                    path += $"/{result}";
                }
            }
            else
            {
                if (pathSegment is { Text: not null })
                {
                    path += $"/{pathSegment.Text}";
                }
            }
        }
        return path;
    }
}