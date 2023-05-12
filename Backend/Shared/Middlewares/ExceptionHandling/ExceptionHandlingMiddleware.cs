using System.Net;
using Microsoft.AspNetCore.Http;
using Middlewares.ExceptionHandling.Exceptions;

namespace Middlewares.ExceptionHandling;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadRequestException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(exception.Message);
        }
        catch (UnauthorizedException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}