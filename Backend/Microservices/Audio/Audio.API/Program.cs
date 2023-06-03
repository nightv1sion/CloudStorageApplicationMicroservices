using Audio.API.DataTransferObjects;
using Audio.API.Exceptions;
using Audio.API.Extensions;
using Audio.API.Model;
using Audio.API.Services;
using Audio.API.Services.Contracts;
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

app.MapPost("api/upload-audio-file", async (
    AudioFileDTO dto,
    HttpContext httpContext,
    IAuthenticationService authenticationService,
    IAudioFileService audioFileService) =>
{
    var userId = authenticationService.GetUserIdFromHeaders(httpContext);
    if (userId is null)
    {
        throw new InvalidUserIdHeaderBadRequestException();
    }
    await audioFileService.SaveAudioFileAsync(dto, userId.Value);
    return Results.Ok();
}).Accepts<AudioFileDTO>("multipart/form-data");

app.MapGet("api/audio-file/{id:guid}", async (
    Guid id,
    IAudioFileService audioFileService) =>
{
    var (bytes, fileName) = await audioFileService.GetAudioFileAsync(id);
    return Results.File(bytes, fileDownloadName: fileName);
});

app.Run();