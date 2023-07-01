FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR "/app"
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY "Shared/" "Shared/"
WORKDIR "/src/Files/src"
COPY "/Microservices/Files/src" .
WORKDIR "Presentation"
RUN dotnet restore "Presentation.csproj"

WORKDIR "/src/Files/Tests/Application.UnitTests"
COPY "/Microservices/Files/Tests/Application.UnitTests" .
RUN dotnet test -c Release

WORKDIR "/src/Files/src/Presentation"
RUN dotnet build "Presentation.csproj" -c Release -o "/app/build"

FROM build AS publish
RUN dotnet publish "Presentation.csproj" -c Release -o "/app/publish" /p:UseAppHost=false

FROM base AS final
WORKDIR "/app"
COPY --from=publish "/app/publish" .
ENTRYPOINT ["dotnet", "Presentation.dll"]
