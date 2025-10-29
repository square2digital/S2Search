# S2Search Backend Solution

## Overview

S2Search Backend is a modular, enterprise-grade backend platform for automotive search and management. It is built with .NET 9 and designed for scalability, maintainability, and integration with modern cloud and on-premises environments.

## Projects

- **S2Search.CacheManager**: Caching layer for high-performance data access.
- **S2Search.Backend**: Core backend logic and orchestration.
- **S2Search.Backend.Services**: Service layer for business logic, including queue management and customer operations.
- **S2Search.Backend.Domain**: Domain models, constants, and shared types.
- **S2Search.Backend.Importer**: Data import and integration utilities.

## Technologies

- **.NET 9**
- **C# 13**
- **Azure Storage Queues**
- **Redis**
- **Dapper**
- **REST APIs**

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional for containerization)
- [Redis](https://redis.io/) (optional for caching)

### Build & Run

1. Clone the repository:
```sh
git clone https://github.com/square2digital/S2Search.git
cd S2Search/Backend
```
2. Restore dependencies:
```sh
dotnet restore
```
3. Build the solution:
```sh
dotnet build
```
4. Run a project (example):
```sh
dotnet run --project S2Search.Backend/S2Search.Backend.csproj
```

## Folder Structure

```
Backend/
??? S2Search.CacheManager/
??? S2Search.Backend/
??? S2Search.Backend.Services/
??? S2Search.Backend.Domain/
??? S2Search.Backend.Importer/
```

## Contributing

1. Fork the repository.
2. Create a feature branch.
3. Make your changes and add tests.
4. Ensure all code builds and tests pass.
5. Submit a pull request.

## License

This project is proprietary. See [LICENSE](../LICENSE) for details.

## Maintainers

- Jonathan Gilmartin - [@JGilmartin-S2](https://github.com/JGilmartin-S2)

---
