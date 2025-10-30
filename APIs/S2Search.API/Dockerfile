# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Build stage: use the .NET SDK to restore and publish
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copy the projects from the build context (which is now the solution root)
# We copy all .csproj files to allow project references to be resolved.
# We also copy a potential solution file (.sln) if it exists.
COPY **/*.csproj ./
# NOTE: If you have a solution file, add it here: COPY **/*.sln ./

# 2. Restore all project dependencies
# We use the simple filename as it was copied to the WORKDIR /src
RUN dotnet restore S2Search.Backend.API.csproj

# 3. Copy the rest of the application source code (including all directories like Domain, Services, etc.)
COPY . .

# 4. Publish the specific API project using its relative path from the solution root
RUN dotnet publish APIs/S2Search.API/S2Search.Backend.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage: use the smaller ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Listen on standard HTTP port
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
EXPOSE 443

# Use the dll produced by dotnet publish
ENTRYPOINT ["dotnet", "S2Search.Backend.API.dll"]
