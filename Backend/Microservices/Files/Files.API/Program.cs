using System.Reflection;
using Files.API.DataTransferObjects;
using Files.API.Exceptions;
using Files.API.Extensions;
using Files.API.Mapping;
using Files.API.Model;
using Files.API.Services;
using Files.API.Services.Contracts;
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
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

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

app.Run();