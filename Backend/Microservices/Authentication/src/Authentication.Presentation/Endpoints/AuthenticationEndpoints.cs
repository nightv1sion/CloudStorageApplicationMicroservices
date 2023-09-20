using Authentication.Application.Extensions.Services;
using Authentication.Application.Features.User.Commands.LoginUser;
using Authentication.Application.Features.User.Commands.RefreshToken;
using Authentication.Application.Features.User.Commands.RegisterUser;
using Authentication.Application.Features.User.DataTransferObjects;
using Authentication.Application.Features.User.Queries.ValidateUserAccessToken;
using Authentication.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Presentation.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api");
        
        group.MapPost("/register", async (
            [FromBody] RegisterUserDto dto,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var errors = await mediator.Send(new RegisterUserQuery(dto), cancellationToken);
            if (errors.Count > 0)
            {
                return Results.ValidationProblem(errors, statusCode: StatusCodes.Status422UnprocessableEntity);
            }
            return Results.Ok("User successfully created");
        }).AddEndpointFilter<ValidationFilter<RegisterUserDto>>();

        group.MapPost("/login", async (
            LoginUserDto dto, 
            [FromServices] IMediator mediator,
            [FromServices] ILogger<AuthenticationService> logger,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new LoginUserQuery(dto), cancellationToken);
            return Results.Ok(result);
        }).AddEndpointFilter<ValidationFilter<LoginUserDto>>();

        group.MapPost("/refresh-token", async (
            [FromBody] TokenDto tokenDto,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var newTokenDto = await mediator.Send(new RefreshTokenQuery(tokenDto), cancellationToken);
            return Results.Ok(newTokenDto);
        }).AddEndpointFilter<ValidationFilter<TokenDto>>();

        group.MapPost("/validate-user", async (
            [FromBody] string accessToken,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = await mediator.Send(new ValidateUserAccessTokenQuery(accessToken), cancellationToken);
            if (userId is not null)
            {
                return Results.Ok(new { UserId = userId.Value });
            }

            return Results.Unauthorized();
        });
    }
}