using FileStorage.API.DataTransferObjects;
using FileStorage.API.Exceptions;
using FileStorage.API.Extensions;
using FileStorage.API.Model;
using FileStorage.API.Services;
using FileStorage.API.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Middlewares.ExceptionHandling;
using Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureServices();

var app = builder.Build();

app.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapGet("api/file/{id:guid}/download", async (
    Guid id,
    IFileService fileService) =>
{
    var (bytes, fileName) = await fileService.GetFileBytesAsync(id);
    return Results.File(bytes, fileDownloadName: fileName);
});

app.MapPost("api/file/upload", async (
    FileDto dto,
    HttpContext httpContext,
    IAuthenticationService authenticationService,
    IFileService fileService) =>
{
    var userId = authenticationService.GetUserIdFromHeaders(httpContext);
    if (userId is null)
    {
        throw new InvalidUserIdHeaderBadRequestException();
    }
    await fileService.SaveFileToStorageAsync(dto, userId.Value);
    return Results.Ok();
}).Accepts<FileDto>("multipart/form-data");

app.Run();