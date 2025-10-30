#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Use the official .NET 9 ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
LABEL author="Square2 Digital"

WORKDIR /app

# Copy published output from build stage (assumes you use 'dotnet publish' in your build pipeline)
COPY . .

EXPOSE 80
EXPOSE 443

# Use the correct DLL name for your main project (update if needed)
ENTRYPOINT ["dotnet", "S2Search.Backend.API"]