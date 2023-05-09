using System.Net;
using FluentValidation;

namespace AuthenticationMicroservice.Filters;

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

        var validationResult = await validator.ValidateAsync(objectToValidate);
        if (validationResult.IsValid)
        {
            return await next.Invoke(context);
        }

        return Results.ValidationProblem(validationResult.ToDictionary(),
            statusCode: (int)HttpStatusCode.UnprocessableEntity);
    }
}