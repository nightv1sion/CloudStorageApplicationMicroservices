FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY Shared/ Shared/
WORKDIR /src
COPY ["/Microservices/Files/Files.API/Files.API.csproj", "Files/Files.API/"]
WORKDIR /src/Files/Files.API
RUN dotnet restore "Files.API.csproj"
COPY /Microservices/Files/Files.API .

WORKDIR /src/Files/Files.API
RUN dotnet build "Files.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Files.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Files.API.dll"]
