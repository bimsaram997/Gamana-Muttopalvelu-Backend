# Stage 1: Base runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj from root directly to current workdir (/src)
COPY ["Gamana-Muttopalvelu-Backend.csproj", "./"]
RUN dotnet restore "Gamana-Muttopalvelu-Backend.csproj"

# Copy all remaining source files and build
COPY . .
RUN dotnet build "Gamana-Muttopalvelu-Backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish output binaries
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Gamana-Muttopalvelu-Backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gamana-Muttopalvelu-Backend.dll"]