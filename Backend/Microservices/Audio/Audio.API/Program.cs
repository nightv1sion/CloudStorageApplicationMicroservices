using Audio.API.Extensions;
using Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapPost("api/test-endpoint", (
    HttpContext context, 
    IAuthenticationService authenticationService) =>
{
    var userId = authenticationService.GetUserIdFromHeaders(context);
    return Results.Ok(userId.Value);
});

app.Run();