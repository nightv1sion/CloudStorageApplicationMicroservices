using Files.Application.Features.File.Commands.CreateFile;
using Files.Application.Features.File.Commands.RemoveFile;
using Files.Application.Features.File.Commands.UpdateFile;
using Files.Application.Features.File.DataTransferObjects;
using Files.Application.Features.File.Queries.DownloadFile;
using Files.Application.Features.File.Queries.GetFile;
using Files.Application.Features.File.Queries.GetFiles;
using Files.Application.Extensions.Services;
using Files.Presentation.Filters;
using MediatR;
using Services.Authentication;

namespace Files.Presentation.Endpoints.Files;

public static class FilesEndpoints
{
    public static WebApplication MapFilesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/file");
        
        group.MapGet("{id:guid}", async (
            Guid id,
            HttpContext httpContext,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = AuthenticationUtilities.GetUserId(httpContext);
            var file = await mediator.Send(
                new GetFileQuery(userId, null, id), cancellationToken);
            return Results.Ok(file);
        });

        group.MapGet("", async (
            HttpContext httpContext,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = AuthenticationUtilities.GetUserId(httpContext);
            var file = await mediator.Send(
                new GetFilesQuery(userId, null), cancellationToken);
            return Results.Ok(file);
        });

        group.MapPut("", async (
            UpdateFileDto dto,
            HttpContext httpContext,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = AuthenticationUtilities.GetUserId(httpContext);
            var file = await mediator.Send(new UpdateFileCommand(userId, dto), cancellationToken);
            return Results.Ok(file);
        }).AddEndpointFilter<ValidationFilter<UpdateFileDto>>();

        group.MapPost("upload", async (
            FormFileDto dto,
            HttpContext httpContext,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = AuthenticationUtilities.GetUserId(httpContext);
            var file = await mediator.Send(new UploadFileCommand(userId, dto), cancellationToken);
            return Results.Ok(file);
        }).Accepts<FormFileDto>("multipart/form-data").AddEndpointFilter<ValidationFilter<FormFileDto>>();

        group.MapGet("{id:guid}/download", async (
            Guid id,
            HttpContext httpContext,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = AuthenticationUtilities.GetUserId(httpContext);
            var dto = await mediator.Send(new DownloadFileQuery(userId, null, id), cancellationToken);
            return Results.File(dto.Bytes, "multipart/form-data", dto.Name + dto.Extension);
        });

        group.MapDelete("{id}", async (
            Guid id,
            IMediator mediator,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var userId = AuthenticationUtilities.GetUserId(httpContext);
            await mediator.Send(new RemoveFileCommand(userId, null, id), cancellationToken);
            return Results.Ok();
        });

        return app;
    }

    public static WebApplication MapDirectoryFilesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/directory/{directoryId:guid}/file");
        
        group.MapGet("{id:guid}", async (
            Guid id,
            Guid directoryId,
            HttpContext httpContext,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = AuthenticationUtilities.GetUserId(httpContext);
            var file = await mediator.Send(new GetFileQuery(userId, directoryId, id), cancellationToken);
            return Results.Ok(file);
        });

        group.MapGet("", async (
            Guid directoryId,
            HttpContext httpContext,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = AuthenticationUtilities.GetUserId(httpContext);
            var file = await mediator.Send(
                new GetFilesQuery(userId, directoryId), cancellationToken);
            return Results.Ok(file);
        });

        group.MapGet("{id:guid}/download", async (
            Guid id,
            Guid directoryId,
            HttpContext httpContext,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = AuthenticationUtilities.GetUserId(httpContext);
            var dto = await mediator.Send(
                new DownloadFileQuery(userId, directoryId, id), cancellationToken);
            return Results.File(dto.Bytes, "multipart/form-data", dto.Name + dto.Extension);
        });

        group.MapDelete("{id}", async (
            Guid id,
            Guid directoryId,
            IMediator mediator,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var userId = AuthenticationUtilities.GetUserId(httpContext);
            await mediator.Send(new RemoveFileCommand(userId, directoryId, id), cancellationToken);
            return Results.Ok();
        });

        return app;
    }
}