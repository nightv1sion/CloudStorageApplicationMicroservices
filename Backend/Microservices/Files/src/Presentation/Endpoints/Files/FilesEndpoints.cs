using Files.Application.Features.File.DataTransferObjects;
using Files.Application.Features.File.Services;
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
            IAuthenticationService authenticationService,
            IFileService fileService,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var file = await fileService.GetFileAsync(userId, id, cancellationToken);
            return Results.Ok(file);
        });

        group.MapGet("", async (
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IFileService fileService,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var file = await fileService.GetFilesAsync(userId, false, cancellationToken);
            return Results.Ok(file);
        });

        group.MapPut("", async (
            UpdateFileDto dto,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IFileService fileService,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            await fileService.UpdateFileAsync(userId, dto, cancellationToken);
            return Results.Ok();
        });

        group.MapPost("upload", async (
            FormFileDto dto,
            HttpContext httpContext,
            IFileService fileService,
            IAuthenticationService authenticationService,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            await fileService.UploadFileAsync(dto, userId, cancellationToken);
            return Results.Ok();
        }).Accepts<FormFileDto>("multipart/form-data");

        group.MapGet("{id:guid}/download", async (
            Guid id,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IFileService fileService,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var dto = await fileService.DownloadFileAsync(userId, id, cancellationToken);
            return Results.File(dto.Bytes, "multipart/form-data", dto.Name + dto.Extension);
        });

        group.MapDelete("{id}", async (
            Guid id,
            IFileService fileService,
            IAuthenticationService authenticationService,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            await fileService.DeleteFileAsync(userId, id, cancellationToken);
            return Results.Ok();
        });

        return app;
    }
}