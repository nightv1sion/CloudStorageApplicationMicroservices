using Files.Presentation.Endpoints.Directories;
using Files.Presentation.Endpoints.Files;
using Files.Presentation.Extensions;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureBuilder();

var app = builder
    .Build()
    .ConfigureApplication();

app.MapFilesEndpoints();
app.MapDirectoryFilesEndpoints();

app.MapDirectoriesEndpoints();

app.Run();