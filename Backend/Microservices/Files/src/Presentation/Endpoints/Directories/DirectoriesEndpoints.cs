using Files.Application.Features.Directory.Commands.CreateDirectory;
using Files.Application.Features.Directory.Commands.RemoveDirectory;
using Files.Application.Features.Directory.Commands.UpdateDirectory;
using Files.Application.Features.Directory.DataTransferObjects;
using Files.Application.Features.Directory.Queries.GetDirectories;
using Files.Application.Features.Directory.Queries.GetDirectory;
using Files.Application.Features.Directory.Validators;
using Files.Presentation.Filters;
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
            var directory = await mediator.Send(new GetDirectoryQuery(userId, null, id), cancellationToken);
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
                new GetDirectoriesQuery(userId, null), cancellationToken);
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
            var directory = await mediator.Send(new CreateDirectoryCommand(userId, dto), cancellationToken);
            return Results.Ok(directory);
        }).AddEndpointFilter<ValidationFilter<CreateDirectoryDto>>();

        group.MapPut("", async (
            UpdateDirectoryDto dto,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var directory = await mediator.Send(
                new UpdateDirectoryCommand(userId, dto), cancellationToken);
            return Results.Ok(directory);
        }).AddEndpointFilter<ValidationFilter<UpdateDirectoryDto>>();

        group.MapDelete("{id:guid}", async (
            Guid id,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            await mediator.Send(new RemoveDirectoryCommand(userId,null, id), cancellationToken);
            return Results.Ok();
        });

        return app;
    }
    
        public static WebApplication MapDirectoriesByParentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/parent-directory/{parentId:guid}/directory");

        group.MapGet("{id:guid}", async (
            Guid id,
            Guid parentId,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var directory = await mediator.Send(new GetDirectoryQuery(userId, parentId, id), cancellationToken);
            return Results.Ok(directory);
        });

        group.MapGet("", async (
            Guid parentId,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var directories = await mediator.Send(
                new GetDirectoriesQuery(userId, parentId), cancellationToken);
            return Results.Ok(directories);
        });
        
        group.MapDelete("{id:guid}", async (
            Guid id,
            Guid parentId,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            await mediator.Send(new RemoveDirectoryCommand(userId, parentId, id), cancellationToken);
            return Results.Ok();
        });

        return app;
    }

}