using Authentication.Presentation.Endpoints;
using Authentication.Presentation.Extensions;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureBuilder();

var app = builder
    .Build()
    .ConfigureApplication();

app.MapAuthenticationEndpoints();

app.Run();

