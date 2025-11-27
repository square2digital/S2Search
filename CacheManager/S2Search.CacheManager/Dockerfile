# -------------------------------------------------------------------------
# Build stage: use the .NET SDK to restore and publish
# -------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copy the entire solution structure (Domain, Services, APIs folders)
COPY . .

# 2. Restore all project dependencies by referencing the API project
RUN dotnet restore APIs/S2Search.API/S2Search.CacheManager.csproj

# 3. Publish the specific API project
RUN dotnet publish APIs/S2Search.API/S2Search.CacheManager.csproj -c Release -o /app/publish /p:UseAppHost=false

# -------------------------------------------------------------------------
# Runtime stage: use the smaller ASP.NET runtime image
# -------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# ** SECURITY IMPROVEMENT 1: Use a non-root user **
# This ensures the container process does not run with elevated privileges.
# The 'app' user is often built into the base image.
USER app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Use the dll produced by dotnet publish
ENTRYPOINT ["dotnet", "CacheManager.dll"]