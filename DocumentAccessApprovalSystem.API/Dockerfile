# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DocumentAccessApprovalSystem.API/DocumentAccessApprovalSystem.API.csproj", "DocumentAccessApprovalSystem.API/"]
COPY ["DocumentAccessApprovalSystem.Application/DocumentAccessApprovalSystem.Application.csproj", "DocumentAccessApprovalSystem.Application/"]
COPY ["DocumentAccessApprovalSystem.Domain/DocumentAccessApprovalSystem.Domain.csproj", "DocumentAccessApprovalSystem.Domain/"]
COPY ["DocumentAccessApprovalSystem.Infrastructure/DocumentAccessApprovalSystem.Infrastructure.csproj", "DocumentAccessApprovalSystem.Infrastructure/"]
RUN dotnet restore "./DocumentAccessApprovalSystem.API/DocumentAccessApprovalSystem.API.csproj"
COPY . .
WORKDIR "/src/DocumentAccessApprovalSystem.API"
RUN dotnet build "./DocumentAccessApprovalSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DocumentAccessApprovalSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DocumentAccessApprovalSystem.API.dll"]