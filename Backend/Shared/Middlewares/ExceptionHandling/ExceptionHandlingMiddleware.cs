using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Middlewares.ExceptionHandling.Exceptions;

namespace Middlewares.ExceptionHandling;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        Exception ex = null;
        try
        {
            await _next(context);
        }
        catch (BadRequestException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(exception.Message);
            ex = exception;
        }
        catch (UnauthorizedException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            ex = exception;
        }
        catch (Exception exception)
        {
            ex = exception;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        if (ex is not null)
        {
            _logger.LogError(ex.ToString());
        }
    }
}