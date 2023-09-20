FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR "/app"
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY "Shared/" "Shared/"
WORKDIR "/src/Files/src"
COPY "/Microservices/Files/src" .
WORKDIR "Files.Presentation"
RUN dotnet restore "Files.Presentation.csproj"

WORKDIR "/src/Files/Tests/Files.Application.UnitTests"
COPY "/Microservices/Files/Tests/Files.Application.UnitTests" .
RUN dotnet test -c Release

WORKDIR "/src/Files/src/Files.Presentation"
RUN dotnet build "Files.Presentation.csproj" -c Release -o "/app/build"

FROM build AS publish
RUN dotnet publish "Files.Presentation.csproj" -c Release -o "/app/publish" /p:UseAppHost=false

FROM base AS final
WORKDIR "/app"
COPY --from=publish "/app/publish" .
ENTRYPOINT ["dotnet", "Files.Presentation.dll"]
