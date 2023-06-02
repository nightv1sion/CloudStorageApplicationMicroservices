using Audio.API.DataTransferObjects;
using Audio.API.Exceptions;
using Audio.API.Extensions;
using Audio.API.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Middlewares.ExceptionHandling;
using Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureAuthentication();
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

app.MapPost("api/test-endpoint", (
    HttpContext context, 
    IAuthenticationService authenticationService) =>
{
    var userId = authenticationService.GetUserIdFromHeaders(context);
    return Results.Ok(userId.Value);
});

app.MapPost("api/upload-audio-file", async (
    HttpContext httpContext,
    IAuthenticationService authenticationService,
    ApplicationDatabaseContext context) =>
{
    var userId = authenticationService.GetUserIdFromHeaders(httpContext);
    if (userId is null)
    {
        throw new InvalidUserIdHeaderBadRequestException();
    }

    var file = httpContext.Request.Form.Files.GetFile("File");
    if (file == null)
    {
        throw new InvalidFileBadRequestException();
    }
    
    var name = httpContext.Request.Form["Name"];
    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
    var fileExtension = Path.GetExtension(file.FileName);
    var audioFile = new AudioFile()
    {
        Name = name,
        FileSystemName = fileName,
        Extension = fileExtension,
        UserId = userId.Value,
    };
    var path = Directory.GetCurrentDirectory();
    await using var stream = new FileStream(path, FileMode.Create);
    await file.CopyToAsync(stream);
    context.AudioFiles.Add(audioFile);
    await context.SaveChangesAsync();
    return Results.Ok();
}).Accepts<AudioFileDTO>("multipart/form-data");

app.Run();