using System.Reflection;
using AuthenticationMicroservice.DataTransferObjects;
using AuthenticationMicroservice.Extensions;
using AuthenticationMicroservice.Filters;
using AuthenticationMicroservice.Model;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapPost("/register", async (
    RegisterUserDTO dto,
    [FromServices] UserManager<User> userManager) =>
{
    var userExists = await userManager.FindByNameAsync(dto.Username);
    if (userExists is not null)
    {
        return Results.BadRequest("User with the same username already exists");
    }

    var user = new User()
    {
        UserName = dto.Username,
        Email = dto.EmailAddress,
    };
    var result = await userManager.CreateAsync(user, dto.Password);
    if (!result.Succeeded)
    {
        return Results.BadRequest("Something went wrong. Please check your credentials");
    }

    return Results.Ok("User successfully created");
}).AddEndpointFilter<ValidationFilter<RegisterUserDTO>>();

app.Run();
