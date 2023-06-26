using Files.API.DataTransferObjects.Directory;
using Files.API.DataTransferObjects.File;
using Files.API.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Services.Authentication;

namespace Files.API.Extensions;

public static class MinimalApiExtensions
{
    public static void MapMinimalApiEndpoints(this WebApplication app)
    {
        app.MapFileMinimalApiEndpoints();   
        app.MapDirectoryMinimalApiEndpoints();
    }
    private static void MapDirectoryMinimalApiEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/directory");

        group.MapGet("", async (
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IDirectoryService directoryService) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var directories = await directoryService.GetDirectoriesAsync(userId);
            return Results.Ok(directories);
        });
        
        group.MapPost("", async (
            CreateDirectoryDto dto,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IDirectoryService directoryService) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            await directoryService.CreateDirectoryAsync(userId, dto);
            return Results.Ok();
        });
    }
    private static void MapFileMinimalApiEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/file");
        
        group.MapGet("{id:guid}", async (
            Guid id,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IFileService fileService) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var file = await fileService.GetFileAsync(userId, id);
            return Results.Ok(file);
        });

        group.MapGet("", async (
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IFileService fileService) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var file = await fileService.GetFilesByUserIdAsync(userId);
            return Results.Ok(file);
        });

        group.MapPut("", async (
            UpdateFileDto dto,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IFileService fileService) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            await fileService.UpdateFileAsync(userId, dto);
            return Results.Ok();
        });

        group.MapPost("upload", async (
            FormFileDto dto,
            HttpContext httpContext,
            IFileService fileService,
            IAuthenticationService authenticationService) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            await fileService.UploadFileAsync(dto, userId);
            return Results.Ok();
        }).Accepts<FormFileDto>("multipart/form-data");

        group.MapGet("{id:guid}/download", async (
            Guid id,
            HttpContext httpContext,
            IAuthenticationService authenticationService,
            IFileService fileService) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            var dto = await fileService.DownloadFileAsync(userId, id);
            return Results.File(dto.Bytes, "multipart/form-data", dto.Name + dto.Extension);
        });

        group.MapDelete("{id}", async (
            Guid id,
            IFileService fileService,
            IAuthenticationService authenticationService,
            HttpContext httpContext) =>
        {
            var userId = authenticationService.GetUserIdFromHeaders(httpContext);
            await fileService.DeleteFileAsync(userId, id);
            return Results.Ok();
        });
    }
}