# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Build stage: use the .NET SDK to restore and publish
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
LABEL author="Square2 Digital"
WORKDIR /src

# Copy everything and restore/publish the API project
COPY . .
RUN dotnet restore "APIs/S2Search.API/S2Search.Backend.API.csproj"
RUN dotnet publish "APIs/S2Search.API/S2Search.Backend.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

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