#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
LABEL author="S2Digital"
COPY . app/
EXPOSE 80
EXPOSE 443

WORKDIR /app
ENTRYPOINT ["dotnet", "Search.dll"]