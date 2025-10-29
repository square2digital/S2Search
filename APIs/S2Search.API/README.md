# S2Search Backend Solution

## Overview

S2Search is a modular backend platform for automotive search and management. The codebase targets .NET9 and is organized into multiple projects (APIs, services, domain, importer, cache manager, and Azure Functions).

## Projects (current)

- `S2Search.Backend.API` - API project for backend HTTP endpoints. Path: `APIs/S2Search.API/S2Search.Backend.API.csproj`
- `S2Search.Backend.Services` - Service layer containing business logic and queue management. Path: `Services/S2Search.Backend.Services/S2Search.Backend.Services.csproj`
- `S2Search.CacheManager` - Caching layer for Redis and in-memory cache. Path: `CacheManager/S2Search.CacheManager/S2Search.CacheManager.csproj`
- `S2Search.Backend.Domain` - Domain models, constants, and shared types. Path: `Domain/S2Search.Backend.Domain/S2Search.Backend.Domain.csproj`
- `S2Search.Backend.Importer` - Importer and integration utilities. Path: `Importer/S2Search.Backend.Importer/S2Search.Backend.Importer.csproj`
- `S2Search.Function.FeedServices` - Azure Function(s) for feed processing. Path: `AzureFunctions/S2Search.Function.FeedServices/S2Search.Function.FeedServices.csproj`
- `S2Search.Functions.SearchInsights` - Azure Function(s) for search insights and telemetry. Path: `AzureFunctions/S2Search.Functions.SearchInsights/S2Search.Functions.SearchInsights.csproj`

> Note: Project paths above are relative to the repository root.

## Technologies

- .NET9
- C# (latest supported by .NET9)
- Azure Storage Queues (optional)
- Redis (optional)
- Dapper
- REST APIs

## Prerequisites

- .NET9 SDK: https://dotnet.microsoft.com/download
- Visual Studio2022+ or VS Code
- Docker Desktop (optional)
- Redis (optional for caching)

## Build & Run

1. Clone the repository:

    ```sh
    git clone https://github.com/square2digital/S2Search.git
    cd S2Search
    ```

2. Restore dependencies:

    ```sh
    dotnet restore
    ```

3. Build the solution:

    ```sh
    dotnet build
    ```

4. Run an individual project (example - API):

    ```sh
    dotnet run --project APIs/S2Search.API/S2Search.Backend.API.csproj
    ```

5. Run services or functions in a similar way by pointing `dotnet run` to the relevant `.csproj` file.

## Folder Structure (high-level)

```
S2Search/
?? APIs/
? ?? S2Search.API/ # API project
?? Services/
? ?? S2Search.Backend.Services/ # Business services
?? CacheManager/
? ?? S2Search.CacheManager/ # Caching layer
?? Domain/
? ?? S2Search.Backend.Domain/ # Domain models and shared types
?? Importer/
? ?? S2Search.Backend.Importer/ # Importer utilities
?? AzureFunctions/
? ?? S2Search.Function.FeedServices/
? ?? S2Search.Functions.SearchInsights/
```

## Contributing

1. Fork the repository.
2. Create a feature branch.
3. Make changes and add tests where appropriate.
4. Ensure `dotnet build` succeeds.
5. Open a pull request.

## License

This project is proprietary. See `LICENSE` in the repository root for details.

## Maintainers

- Jonathan Gilmartin - @JGilmartin-S2
