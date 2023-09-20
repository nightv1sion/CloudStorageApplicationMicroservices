FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR "/app"
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY "Shared/" "Shared/"
WORKDIR "/src/Authentication/src"
COPY "/Microservices/Authentication/src" .
WORKDIR "Authentication.Presentation"
RUN dotnet restore "Authentication.Presentation.csproj"

WORKDIR "/src/Authentication/Tests/Authentication.Application.UnitTests"
COPY "/Microservices/Authentication/Tests/Authentication.Application.UnitTests" .
RUN dotnet test -c Release

WORKDIR "/src/Authentication/src/Authentication.Presentation"
RUN dotnet build "Authentication.Presentation.csproj" -c Release -o "/app/build"

FROM build AS publish
RUN dotnet publish "Authentication.Presentation.csproj" -c Release -o "/app/publish" /p:UseAppHost=false

FROM base AS final
WORKDIR "/app"
COPY --from=publish "/app/publish" .
ENTRYPOINT ["dotnet", "Authentication.Presentation.dll"]
