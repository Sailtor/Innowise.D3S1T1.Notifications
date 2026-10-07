# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["src/Notifications.AppHost/Notifications.AppHost.csproj", "src/Notifications.AppHost/"]
COPY ["src/Notifications.Application/Notifications.Application.csproj", "src/Notifications.Application/"]
COPY ["src/Notifications.Domain/Notifications.Domain.csproj", "src/Notifications.Domain/"]
COPY ["src/Notifications.Infrastructure/Notifications.Infrastructure.csproj", "src/Notifications.Infrastructure/"]
COPY ["src/Notifications.Presentation/Notifications.Presentation.csproj", "src/Notifications.Presentation/"]
RUN --mount=type=secret,id=github_token \
    GITHUB_TOKEN=$(cat /run/secrets/github_token) \
    dotnet restore "src/Notifications.AppHost/Notifications.AppHost.csproj"
COPY . .
RUN dotnet build "src/Notifications.AppHost/Notifications.AppHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/Notifications.AppHost/Notifications.AppHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notifications.AppHost.dll"]
