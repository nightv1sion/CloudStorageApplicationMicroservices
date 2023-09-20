using Middlewares.ExceptionHandling.Exceptions;

namespace Authentication.Application.Common.Exceptions.Unauthorized;

public class InvalidUserNameUnauthorized : UnauthorizedException
{
    public InvalidUserNameUnauthorized(string username) : base(
        $"User with username '{username}' does not exist")
    {

    }
}