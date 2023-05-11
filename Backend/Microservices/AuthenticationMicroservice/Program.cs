using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using AuthenticationMicroservice.DataTransferObjects;
using AuthenticationMicroservice.Extensions;
using AuthenticationMicroservice.Filters;
using AuthenticationMicroservice.Model;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AuthenticationMicroservice.Services;
using AuthenticationMicroservice.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
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

app.MapPost("/api/register", async (
    RegisterUserDTO dto,
    IAuthenticationService authenticationService) =>
{
    var result = await authenticationService.RegisterUserAsync(dto);
    if (!result.Succeeded)
    {
        return Results.ValidationProblem(result.Errors.GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, y => y.Select(element => element.Description).ToArray()), 
            statusCode: StatusCodes.Status422UnprocessableEntity);
    }

    return Results.Ok("User successfully created");
}).AddEndpointFilter<ValidationFilter<RegisterUserDTO>>();

app.MapPost("api/login", async (
    LoginUserDTO dto, 
    IAuthenticationService _authenticationService) =>
{
    var result = await _authenticationService.LoginUserAsync(dto);
    if (result == null)
    {
        return Results.Unauthorized();
    }
    return Results.Ok(new
    {
        Token = new JwtSecurityTokenHandler().WriteToken(result.Token),
        RefreshToken = result.RefreshToken,
        Expiration = result.ValidTo
    });
;}).AddEndpointFilter<ValidationFilter<LoginUserDTO>>();

app.Run();

