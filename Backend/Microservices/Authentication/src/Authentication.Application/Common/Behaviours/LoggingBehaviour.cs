using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Authentication.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestGuid = Guid.NewGuid();
        var requestJsonData = JsonSerializer.Serialize(request);

        _logger.LogInformation(
            $"[MEDIATOR] [START] Request Id: {requestGuid}; Request Name: {requestName}; Processed Data: {requestJsonData}");

        var response = await next();
        
        _logger.LogInformation(
            $"[MEDIATOR] [END] Request Id: {requestGuid}; Request Name: {requestName}");

        return response;
    }
}