FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY Shared/ Shared/
WORKDIR /src
COPY ["/Microservices/Audio/Audio.API/Audio.API.csproj", "Audio/Audio.API/"]
WORKDIR /src/Audio/Audio.API
RUN dotnet restore "Audio.API.csproj"
COPY /Microservices/Audio/Audio.API .

WORKDIR /src/Audio/Audio.API
RUN dotnet build "Audio.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Audio.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Audio.API.dll"]
