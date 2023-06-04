FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY Shared/ Shared/
WORKDIR /src
COPY ["/Microservices/FileStorage/FileStorage.API/FileStorage.API.csproj", "FileStorage/FileStorage.API/"]
WORKDIR /src/FileStorage/FileStorage.API
RUN dotnet restore "FileStorage.API.csproj"
COPY /Microservices/FileStorage/FileStorage.API .

WORKDIR /src/FileStorage/FileStorage.API
RUN dotnet build "FileStorage.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileStorage.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileStorage.API.dll"]
