# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["R3M.Financas.Api/cert.pem", "cert.pem"]
COPY ["R3M.Financas.Api/R3M.Financas.Api.csproj", "R3M.Financas.Api/"]
COPY ["R3M.Financas.Client/R3M.Financas.Client.csproj", "R3M.Financas.Client/"]
COPY ["R3M.Financas.Shared/R3M.Financas.Shared.csproj", "R3M.Financas.Shared/"]
RUN dotnet restore "./R3M.Financas.Api/R3M.Financas.Api.csproj"
COPY . .
WORKDIR "/src/R3M.Financas.Api"
RUN dotnet build "./R3M.Financas.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./R3M.Financas.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "R3M.Financas.Api.dll"]