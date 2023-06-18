FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR "/app"
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY "Shared/" "Shared/"
WORKDIR "/src/Files/Files.API"
COPY "/Microservices/Files/Files.API/Files.API.csproj" .
RUN dotnet restore "Files.API.csproj"
COPY "/Microservices/Files/Files.API" .

WORKDIR "/src/Files/Tests/Files.API.UnitTests"
COPY "/Microservices/Files/Tests/Files.API.UnitTests" .
RUN dotnet restore "Files.API.UnitTests.csproj"
RUN dotnet test "Files.API.UnitTests.csproj" -c Release

WORKDIR "/src/Files/Files.API"
RUN dotnet build "Files.API.csproj" -c Release -o "/app/build"

FROM build AS publish
RUN dotnet publish "Files.API.csproj" -c Release -o "/app/publish" /p:UseAppHost=false

FROM base AS final
WORKDIR "/app"
COPY --from=publish "/app/publish" .
ENTRYPOINT ["dotnet", "Files.API.dll"]
