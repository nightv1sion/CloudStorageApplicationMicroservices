FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR "/app"
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR "/src"
COPY "/Shared" "Shared/"
WORKDIR "Microservices/Storage/Storage.API"
COPY "Microservices/Storage/Storage.API" .
RUN dotnet restore "Storage.API.csproj"

WORKDIR "/src/Microservices/Storage/Tests/Storage.API.UnitTests"
COPY "/Microservices/Storage/Tests/Storage.API.UnitTests" .
RUN dotnet restore "Storage.API.UnitTests.csproj"
RUN dotnet test "Storage.API.UnitTests.csproj" -c Release

COPY . .
WORKDIR "/src/Microservices/Storage/Storage.API"
RUN dotnet build "Storage.API.csproj" -c Release -o "/app/build"

FROM build AS publish
RUN dotnet publish "Storage.API.csproj" -c Release -o "/app/publish" /p:UseAppHost=false

FROM base AS final
WORKDIR "/app"
COPY --from=publish "/app/publish" .
ENTRYPOINT ["dotnet", "Storage.API.dll"]
