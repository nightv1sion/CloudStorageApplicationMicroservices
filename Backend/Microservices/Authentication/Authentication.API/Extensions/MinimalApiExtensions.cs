using System.IdentityModel.Tokens.Jwt;
using Authentication.API.DataTransferObjects;
using Authentication.API.Filters;
using Authentication.API.Services;
using Authentication.API.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Extensions;

public static class MinimalApiExtensions
{
    public static void MapAuthenticationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api");
        
        group.MapPost("/register", async (
            RegisterUserDTO dto,
            IAuthenticationService authenticationService) =>
        {
            var result = await authenticationService.RegisterUserAsync(dto);
            if (!result.Succeeded)
            {
                return Results.ValidationProblem(result.Errors.GroupBy(x => x.Code)
                        .ToDictionary(x => x.Key, y => y.Select(
                            element => element.Description).ToArray()), 
                    statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            return Results.Ok("User successfully created");
        }).AddEndpointFilter<ValidationFilter<RegisterUserDTO>>();

        group.MapPost("/login", async (
            LoginUserDTO dto, 
            IAuthenticationService authenticationService,
            [FromServices] ILogger<AuthenticationService> logger) =>
        {
            var result = await authenticationService.LoginUserAsync(dto);
            return Results.Ok(result);
        }).AddEndpointFilter<ValidationFilter<LoginUserDTO>>();

        group.MapPost("/refresh-token", async (
            TokenDTO tokenDto,
            IAuthenticationService authenticationService) =>
        {
            var newTokenDto = await authenticationService.GetRefreshTokenAsync(tokenDto);
            return Results.Ok(newTokenDto);
        }).AddEndpointFilter<ValidationFilter<TokenDTO>>();

        group.MapPost("/validate-user", async(
            [FromBody]string accessToken,
            IAuthenticationService authenticationService) =>
        {
            var result = authenticationService.ValidateUser(accessToken);
            if (result == true)
            {
                return Results.Ok();
            }
            return Results.Unauthorized();
        });
    }
}