﻿using System.Net;
using System.Net.Http.Headers;
using OcelotApiGateway.DataTransferObjects;
using Routes.Helpers;

namespace OcelotApiGateway.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IRoutePatternHelper _routePatternHelper;

    public AuthenticationMiddleware(
        RequestDelegate next, 
        IConfiguration configuration,
        IRoutePatternHelper routePatternHelper)
    {
        _next = next;
        _httpClient = new HttpClient();
        _configuration = configuration;
        _routePatternHelper = routePatternHelper;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var protectedRoutes = GetProtectedRoutes();
        var currentPath = context.Request.Path;

        OcelotPath? route = null;
        foreach (var protectedRoute in protectedRoutes)
        {
            var values = _routePatternHelper.GetRouteValues(protectedRoute.UpstreamPathTemplate, currentPath);
            var path = _routePatternHelper.SetRouteValuesIntoPattern(protectedRoute.UpstreamPathTemplate, values);
            if (path == currentPath)
            {
                route = protectedRoute;
            }
        }
        
        if (route is null || route.Protected == false)
        {
            await _next(context);
            return;
        }

        var authorizationHeader = context.Request.Headers.Authorization.FirstOrDefault();
        if (authorizationHeader is not null)
        {
            if (AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue))
            {
                var accessToken = headerValue.Parameter;
                if (accessToken is not null)
                {
                    var path = _configuration["AuthenticationMicroservice:ValidateTokenPath"];
                    using var request = new HttpRequestMessage(HttpMethod.Post, path);
                    request.Content = JsonContent.Create(accessToken);;
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await _httpClient.SendAsync(request);
                
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var user = await response.Content.ReadFromJsonAsync<ValidatedUser>();
                        if (user is not null && user.UserId != Guid.Empty)
                        {
                            context.Request.Headers.Add("UserID", user.UserId.ToString());
                            await _next(context);
                            return;
                        }
                    }
                }
            }
        
        }
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.CompleteAsync();
    }

    private IEnumerable<OcelotPath> GetProtectedRoutes()
    {
        var routeSection = _configuration.GetSection("Routes").Get<OcelotPath[]>();
        if (routeSection == null)
        {
            throw new InvalidOperationException("Routes section does not exist in IConfiguration");
        }
        return routeSection.Where(x => x.Protected is true);
    }
}