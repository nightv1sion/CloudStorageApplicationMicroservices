FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY Shared/ Shared/
WORKDIR /src

COPY ["/Microservices/Authentication/Authentication.API/Authentication.API.csproj", "Authentication/Authentication.API/"]
WORKDIR "/src/Authentication/Authentication.API/"
RUN dotnet restore "Authentication.API.csproj"
COPY /Microservices/Authentication/Authentication.API .

WORKDIR "/src/Authentication/Tests/Authentication.UnitTests"
COPY /Microservices/Authentication/Tests/Authentication.UnitTests .
RUN dotnet restore "Authentication.UnitTests.csproj"
RUN dotnet test "Authentication.UnitTests.csproj" -c Release

WORKDIR "/src/Authentication/Authentication.API/"
RUN dotnet build "Authentication.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authentication.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication.API.dll"]
