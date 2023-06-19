using System.Reflection;
using System.Security.Cryptography;
using Files.API.DataTransferObjects;
using Files.API.Extensions;
using Files.API.Services.Contracts;
using Middlewares.ExceptionHandling;
using Serilog;
using Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration)
    => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureServices();
builder.Services.ConfigureMassTransit(builder.Configuration);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapGet("api/file/{id:guid}", async (
    Guid id,
    HttpContext httpContext,
    IAuthenticationService authenticationService,
    IFileService fileService) =>
{
    var userId = authenticationService.GetUserIdFromHeaders(httpContext);
    var file = await fileService.GetFileAsync(userId, id);
    return Results.Ok(file);
});

app.MapGet("api/file", async (
    HttpContext httpContext,
    IAuthenticationService authenticationService,
    IFileService fileService) =>
{
    var userId = authenticationService.GetUserIdFromHeaders(httpContext);
    var file = await fileService.GetFilesByUserIdAsync(userId);
    return Results.Ok(file);
});

app.MapPut("api/file", async (
    UpdateFileDto dto,
    HttpContext httpContext,
    IAuthenticationService authenticationService,
    IFileService fileService) =>
{
    var userId = authenticationService.GetUserIdFromHeaders(httpContext);
    await fileService.UpdateFileAsync(userId, dto);
    return Results.Ok();
});

app.MapPost("api/file/upload", async (
    FormFileDto dto,
    HttpContext httpContext,
    IFileService fileService,
    IAuthenticationService authenticationService) =>
{
    var userId = authenticationService.GetUserIdFromHeaders(httpContext);
    await fileService.UploadFileAsync(dto, userId);
    return Results.Ok();
}).Accepts<FormFileDto>("multipart/form-data");

app.MapGet("api/file/{id:guid}/download", async (
    Guid id,
    HttpContext httpContext,
    IAuthenticationService authenticationService,
    IFileService fileService) =>
{
    var userId = authenticationService.GetUserIdFromHeaders(httpContext);
    var dto = await fileService.DownloadFileAsync(userId, id);
    return Results.File(dto.Bytes, "multipart/form-data", dto.Name + dto.Extension);
});

app.MapDelete("api/file/{id}", async (
    Guid id,
    IFileService fileService,
    IAuthenticationService authenticationService,
    HttpContext httpContext) =>
{
    var userId = authenticationService.GetUserIdFromHeaders(httpContext);
    await fileService.DeleteFileAsync(userId, id);
    return Results.Ok();
});

app.Run();