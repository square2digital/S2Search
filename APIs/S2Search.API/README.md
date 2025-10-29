# S2Search Backend API

## Overview

The main backend API for S2Search, providing a unified gateway for automotive search and management operations. Built with .NET 8 and designed for enterprise-scale deployments with comprehensive caching, validation, and service orchestration.

## Architecture

This API serves as the primary backend service that orchestrates:

- Customer management and authentication
- Search index configuration
- Feed processing coordination via [`FeedServices`](../../AzureFunctions/S2Search.Function.FeedServices)
- Cache management through [`S2Search.CacheManager`](../../CacheManager/S2Search.CacheManager)
- Business logic via [`S2Search.Backend.Services`](../../Services/S2Search.Backend.Services)

## Key Features

- **RESTful API Design** - Clean, documented endpoints
- **Swagger Documentation** - Interactive API documentation
- **Redis Caching** - High-performance data caching
- **Dependency Injection** - Modular service architecture
- **Azure Functions Integration** - Serverless feed processing
- **Database Abstraction** - Support for SQL Server and PostgreSQL

## Technologies

- **.NET 8** - Modern cross-platform framework
- **ASP.NET Core** - High-performance web framework
- **Swagger/OpenAPI** - API documentation
- **Redis** - Distributed caching
- **Dapper** - Lightweight ORM
- **Azure Storage** - Blob and queue services

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Redis](https://redis.io/) (for caching)
- [SQL Server](https://www.microsoft.com/sql-server/) or [PostgreSQL](https://www.postgresql.org/)

### Build & Run

1. **Clone and navigate:**

   ```bash
   git clone https://github.com/square2digital/S2Search.git
   cd S2Search/APIs/S2Search.API
   ```

2. **Restore dependencies:**

   ```bash
   dotnet restore
   ```

3. **Configure settings:**
   Update `appsettings.json` with your connection strings and settings

4. **Build the solution:**

   ```bash
   dotnet build
   ```

5. **Run the API:**

   ```bash
   dotnet run
   ```

6. **Access Swagger UI:**
   Navigate to `https://localhost:5000/swagger`

## Configuration

### Connection Strings

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=S2Search;Trusted_Connection=true;",
    "Redis": "localhost:6379",
    "AzureStorage": "DefaultEndpointsProtocol=https;..."
  }
}
```

### Key Services

The API integrates several core services from [`ServiceCollectionExtension`](../../Services/S2Search.Backend.Services/ServiceCollectionExtension.cs):

- **IFeedRepository** - Feed data management
- **IBlobClientProvider** - Azure Storage operations
- **IFeedSettingsValidationManager** - Feed validation logic
- **ICronDescriptionManager** - CRON expression management

## API Endpoints

### Core Endpoints

- `GET /api/health` - Health check endpoint
- `GET /api/probe/ready` - Readiness probe for Kubernetes
- `POST /api/customers/{id}/feeds` - Feed management
- `GET /api/search/suggestions` - Search autocomplete

### Admin Endpoints

- `POST /api/admin/feeds/upload` - Feed file upload
- `GET /api/admin/dashboard/summary` - Dashboard metrics
- `PUT /api/admin/themes/{id}` - Theme configuration

## Integration Points

### Azure Functions

The API coordinates with several Azure Functions:

- [`FeedMonitor`](../../AzureFunctions/S2Search.Function.FeedServices/FeedMonitor.cs) - File processing monitoring
- [`FeedExtractor`](../../AzureFunctions/S2Search.Function.FeedServices/FeedExtractor.cs) - Archive extraction
- [`FeedValidator`](../../Domain/S2Search.Backend.Domain/AzureFunctions/FeedServices/Constants/FunctionNames.cs) - Data validation
- [`FeedProcessor`](../../Domain/S2Search.Backend.Domain/AzureFunctions/FeedServices/Constants/FunctionNames.cs) - Final processing

### Cache Layer

Integrates with [`S2Search.CacheManager`](../../CacheManager/S2Search.CacheManager) for:

- Search result caching
- Configuration caching
- Session management

## Folder Structure

```
S2Search.API/
├── Controllers/          # API controllers
├── Models/              # Request/response models
├── Services/            # Application services
├── Configuration/       # Startup configuration
├── Middleware/          # Custom middleware
└── Documentation/       # API documentation
```

## Development

### Running Tests

```bash
dotnet test
```

### Code Standards

- Follow .NET coding conventions
- Use async/await patterns for I/O operations
- Implement proper error handling and logging
- Document public APIs with XML comments

### Docker Support

```bash
# Build container
docker build -t s2search-api .

# Run container
docker run -p 5000:80 s2search-api
```

## Deployment

### Kubernetes

Deploy using the provided Kubernetes manifests in [`K8s`](../../K8s) directory:

```bash
kubectl apply -f k8s-deployment.yaml
```

### Environment Variables

Key environment variables for production:

- `ASPNETCORE_ENVIRONMENT` - Environment setting
- `ConnectionStrings__DefaultConnection` - Database connection
- `ConnectionStrings__Redis` - Redis connection
- `AzureStorage__ConnectionString` - Azure Storage

## Contributing

1. Fork the repository
2. Create a feature branch
3. Follow coding standards and add tests
4. Ensure all tests pass
5. Submit a pull request

See [CONTRIBUTING.md](../../CONTRIBUTING.md) for detailed guidelines.

## License

This project is proprietary software. See [LICENSE](../../LICENSE) for details.

## Support

For enterprise support and licensing inquiries:

- **Email**: [info@square2digital.com](mailto:info@square2digital.com)
- **Website**: [Square2 Digital](https://www.square2digital.com/s2-search/)

---

_Built with ❤️ using .NET 8 and ASP.NET Core_
