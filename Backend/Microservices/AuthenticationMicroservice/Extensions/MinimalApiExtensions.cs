using System.IdentityModel.Tokens.Jwt;
using AuthenticationMicroservice.DataTransferObjects;
using AuthenticationMicroservice.Filters;
using AuthenticationMicroservice.Model;
using AuthenticationMicroservice.Services;
using AuthenticationMicroservice.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationMicroservice.Extensions;

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
            IAuthenticationService authenticationService) =>
        {
            var result = await authenticationService.LoginUserAsync(dto);
            return Results.Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(result.Token),
                RefreshToken = result.RefreshToken,
                Expiration = result.ValidTo
            });
        ;}).AddEndpointFilter<ValidationFilter<LoginUserDTO>>();

        group.MapPost("/refresh-token", async (
            TokenDTO tokenDto,
            IAuthenticationService authenticationService) =>
        {
            var newTokenDto = await authenticationService.GetRefreshTokenAsync(tokenDto);
            return Results.Ok(newTokenDto);
        }).AddEndpointFilter<ValidationFilter<TokenDTO>>();
    }
}