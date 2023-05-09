FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY Shared/ Shared/
WORKDIR /src
COPY ["/Microservices/AuthenticationMicroservice/AuthenticationMicroservice.csproj", "AuthenticationMicroservice/"]
WORKDIR "/src/AuthenticationMicroservice"
RUN dotnet restore "AuthenticationMicroservice.csproj"
COPY /Microservices/AuthenticationMicroservice/ .

WORKDIR "/src/AuthenticationMicroservice"
RUN dotnet build "AuthenticationMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthenticationMicroservice.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationMicroservice.dll"]
