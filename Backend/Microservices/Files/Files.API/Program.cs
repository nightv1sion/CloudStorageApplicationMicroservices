using System.Reflection;
using AutoMapper;
using Files.API.DataTransferObjects;
using Files.API.Exceptions;
using Files.API.Extensions;
using Files.API.Mapping;
using Files.API.Model;
using Files.API.Services;
using Files.API.Services.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Middlewares.ExceptionHandling;
using Models.File;
using Services.Authentication;
using File = Files.API.Model.File;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureServices(builder.Configuration);
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

app.MapPost("api/upload", async (
    FormFileDto dto,
    HttpContext httpContext,
    IBus bus,
    IFileService fileService,
    IAuthenticationService authenticationService,
    IMapper mapper,
    IConfiguration configuration) =>
{
    var userId = authenticationService.GetUserIdFromHeaders(httpContext);
    var createFileDto = mapper.Map<CreateFileDto>(dto);
    createFileDto.UserId = userId;
    var file = await fileService.CreateFileAsync(createFileDto);
    await using var stream = new MemoryStream();
    await dto.File.CopyToAsync(stream);
    var bytes = stream.ToArray();
    var fileToStorage = new Models.File.File()
    {
        Bytes = bytes,
        Info = new()
        {
            Name = file.Name,
            Extension = file.Extension,
            FileSystemName = file.Id.ToString()
        }
    };

    var uri = new Uri(configuration["RABBIT_MQ_FILE_QUEUE_PATH"]);
    var endpoint = await bus.GetSendEndpoint(uri);
    await endpoint.Send(fileToStorage);
    return Results.Ok();
}).Accepts<FormFileDto>("multipart/form-data");

app.Run();