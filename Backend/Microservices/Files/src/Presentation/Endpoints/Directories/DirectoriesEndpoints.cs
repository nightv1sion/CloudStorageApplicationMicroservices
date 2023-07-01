using Files.Application.Features.Directory.Commands.CreateUserDirectory;
using Files.Application.Features.Directory.Commands.RemoveUserDirectory;
using Files.Application.Features.Directory.Commands.UpdateUserDirectory;
using Files.Application.Features.Directory.DataTransferObjects;
using Files.Application.Features.Directory.Queries.GetUserDirectories;
using Files.Application.Features.Directory.Queries.GetUserDirectory;
using MediatR;
using Services.Authentication;

namespace Files.Presentation.Endpoints.Directories;

public static class DirectoriesEndpoints
{
    public static WebApplication MapDirectoriesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/directory");

        group.MapGet("{id:guid}", async (
            Guid id,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var directory = await mediator.Send(new GetUserDirectoryQuery(userId, id), cancellationToken);
            return Results.Ok(directory);
        });

        group.MapGet("", async (
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var directories = await mediator.Send(
                new GetUserDirectoriesQuery(userId), cancellationToken);
            return Results.Ok(directories);
        });
        
        group.MapPost("", async (
            CreateDirectoryDto dto,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var directory = await mediator.Send(new CreateUserDirectoryCommand(userId, dto), cancellationToken);
            return Results.Ok(directory);
        });

        group.MapPut("", async (
            UpdateDirectoryDto dto,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var directory = await mediator.Send(
                new UpdateUserDirectoryCommand(userId, dto), cancellationToken);
            return Results.Ok(directory);
        });

        group.MapDelete("{id:guid}", async (
            Guid id,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            await mediator.Send(new RemoveUserDirectoryCommand(userId, id), cancellationToken);
            return Results.Ok();
        });

        return app;
    }
}