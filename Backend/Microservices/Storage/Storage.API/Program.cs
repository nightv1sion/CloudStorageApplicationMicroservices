using Microsoft.AspNetCore.Mvc;
using Storage.API.DataTransferObjects;
using Storage.API.Extensions;
using Storage.API.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureServices();
builder.Services.ConfigureMassTransit(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("api/file/download", async (
    [FromBody]DownloadFileDto dto,
    IStorageService storageService) =>
{
    var bytes = await storageService.GetFileBytesAsync(dto.StorageFileName);
    return Results.File(bytes, fileDownloadName: dto.OriginalName);
});

app.MapPost("api/file/upload", async (
    FormFileDto dto,
    IStorageService storageService) =>
{
    await storageService.SaveFormFileAsync(dto);
    return Results.Ok();
}).Accepts<FormFileDto>("multipart/form-data");

app.Run();