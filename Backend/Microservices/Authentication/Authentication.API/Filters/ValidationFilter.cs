using System.Net;
using System.Text.Json;
using FluentValidation;

namespace Authentication.API.Filters;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        T? objectToValidate = context.GetArgument<T>(0);
        IValidator<T> validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

        if (validator is null)
        {
            throw new NotSupportedException("Service is not provided");
        }

        if (objectToValidate == null)
        {
            return Results.BadRequest("Object can not be null");
        }

        var validationResult = await validator.ValidateAsync(objectToValidate);
        if (validationResult.IsValid)
        {
            return await next.Invoke(context);
        }

        return Results.ValidationProblem(validationResult.ToDictionary(),
            statusCode: (int)HttpStatusCode.UnprocessableEntity);
    }
}