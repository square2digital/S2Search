# S2Search Cache Manager

A .NET 9 console application that monitors an Azure Storage Queue and purges Redis cache entries based on incoming messages. This service enables cache invalidation across distributed S2Search instances when search data is updated.

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Configuration](#configuration)
- [Getting Started](#getting-started)
- [Development](#development)
- [Deployment](#deployment)
- [Project Structure](#project-structure)
- [Testing](#testing)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)

## Overview

The Cache Manager is a background service that:

1. Polls an Azure Storage Queue (`purge-cache`) for cache invalidation messages
2. Decodes and deserializes messages containing host information
3. Deletes matching Redis cache keys using wildcard pattern matching
4. Removes processed messages from the queue

This service ensures that when search indices or configurations change, all cached data is properly invalidated across the application cluster.

## Architecture

```
┌─────────────────┐      ┌──────────────────┐      ┌──────────────────┐
│  Azure Queue    │─────▶│  Cache Manager   │─────▶│  Redis Cache     │
│  (purge-cache)  │      │                  │      │                  │
└─────────────────┘      └──────────────────┘      └──────────────────┘
                                   │
                                   ▼
                         ┌──────────────────┐
                         │  Message Delete  │
                         └──────────────────┘
```

### Components

- **PurgeCacheProcessor**: Main orchestration logic - polls queue, processes messages, coordinates cache deletion
- **QueueManager**: Azure Storage Queue client wrapper for message operations
- **RedisCacheManager**: Redis operations including wildcard-based key deletion
- **Cache Key Generation**: Strips colons from host identifiers and appends trailing colon for wildcard matching

## Features

- **Queue-based cache invalidation** - Decoupled architecture allows any service to trigger cache purging
- **Wildcard pattern matching** - Delete all keys matching a specific host/tenant
- **Batch processing** - Processes up to 25 messages per execution cycle
- **Connection validation** - Tests storage account connectivity before processing
- **Graceful shutdown** - 5-second timeout with cancellation token support
- **Structured logging** - Microsoft.Extensions.Logging (optional ApplicationInsights)
- **Base64 message decoding** - Handles Azure Storage Queue message encoding
- **Error resilience** - Continues processing remaining messages if individual message fails

## Prerequisites

- **.NET 9 SDK** or later
- **Azure Storage Account** with queue `purge-cache` created
- **Redis instance** (local or remote)
- **Visual Studio 2022** or **VS Code** with C# extension (recommended)

## Configuration

### appsettings.json

```json
{
  "ApplicationInsights": {
    "InstrumentationKey": "YOUR_INSTRUMENTATION_KEY"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    },
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning"
      }
    }
  },
  "ConnectionStrings": {
    "AzureStorage": "DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net",
    "Redis": "localhost:6379"
  }
}
```

### Environment-Specific Configuration

- `appsettings.Development.json` - Local development overrides (optional)
- `appsettings.Kubernetes.json` - Kubernetes deployment settings (optional)

If present, these are loaded automatically based on `DOTNET_ENVIRONMENT`.

### Queue Message Format

Messages in the `purge-cache` queue must be JSON with the following structure:

```json
{
  "Host": "example.s2search.co.uk"
}
```

The message body is Base64-encoded by Azure Storage Queue automatically.

## Getting Started

### Local Development

1. **Clone the repository**

   ```bash
   git clone https://github.com/square2digital/S2Search.git
   cd S2Search/CacheManager/S2Search.CacheManager
   ```

2. **Configure connection strings**

   Update `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "AzureStorage": "UseDevelopmentStorage=true",
       "Redis": "localhost:6379"
     }
   }
   ```

3. **Start dependencies**

   ```bash
   # Start Azurite (Azure Storage Emulator)
   azurite --silent --location c:\azurite --debug c:\azurite\debug.log

   # Start Redis
   docker run -d -p 6379:6379 redis:latest
   ```

4. **Run the application**
   ```bash
   dotnet run --project S2Search.CacheManager.csproj
   ```

### Docker

```bash
# Build image
docker build -t s2search-cachemanager:latest -f Dockerfile .

# Run container
docker run -e ConnectionStrings__AzureStorage="..." \
           -e ConnectionStrings__Redis="redis:6379" \
           -e ApplicationInsights__InstrumentationKey="YOUR_KEY" \
           s2search-cachemanager:latest

# Note: ensure Dockerfile base image matches .NET 9 console apps (e.g., mcr.microsoft.com/dotnet/runtime:9.0).
```

## Development

### Building

```bash
dotnet build S2Search.CacheManager.csproj
```

### Adding New Features

1. **Create interface** in `Services/Interfaces/Managers/` or `Services/Interfaces/Processors/`
2. **Implement service** in `Services/Managers/` or `Services/Processors/`
3. **Register in DI** in `Program.cs` `ConfigureServices` method
4. **Update README** with new functionality

### Code Style

- Follow [C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use dependency injection for all services
- Prefer async/await for I/O operations
- Include XML documentation for public APIs

## Deployment

### Kubernetes

The Cache Manager runs as a **CronJob** in Kubernetes to periodically process the queue:

```yaml
apiVersion: batch/v1
kind: CronJob
metadata:
  name: s2search-cachemanager
spec:
  schedule: ""*/5 * * * *""  # Every 5 minutes
  jobTemplate:
    spec:
      template:
        spec:
          containers:
          - name: cachemanager
            image: ghcr.io/square2digital/s2search-cachemanager:latest
            env:
            - name: ConnectionStrings__AzureStorage
              valueFrom:
                secretKeyRef:
                  name: s2search-secrets
                  key: azure-storage-connection
            - name: ConnectionStrings__Redis
              value: "s2search-redis-master:6379"
            - name: ApplicationInsights__InstrumentationKey
              valueFrom:
                secretKeyRef:
                  name: s2search-secrets
                  key: appinsights-key
          restartPolicy: OnFailure
```

### Environment Variables

The following environment variables override `appsettings.json`:

- `ConnectionStrings__AzureStorage` - Azure Storage connection string
- `ConnectionStrings__Redis` - Redis connection string
- `ApplicationInsights__InstrumentationKey` - Application Insights instrumentation key
- `DOTNET_ENVIRONMENT` - Environment name (Development, Staging, Production)

## Project Structure

```
S2Search.CacheManager/
├── appsettings.json                  # Base configuration
├── Dockerfile                        # Container image definition
├── Program.cs                        # Application entry point with DI setup
├── S2Search.CacheManager.csproj      # Project file
├── nuget.config                      # NuGet configuration
├── CacheManager/
│   ├── CacheManager.csproj           # Legacy project file
│   └── Extensions/
│       └── (legacy extension files)
├── Services/
│   ├── Interfaces/
│   │   ├── Managers/
│   │   │   ├── ICacheManager.cs      # Redis cache abstraction
│   │   │   └── IQueueManager.cs      # Azure Queue abstraction
│   │   └── Processors/
│   │       └── IPurgeCacheProcessor.cs  # Main processor interface
│   ├── Managers/
│   │   ├── QueueManager.cs           # Queue message operations
│   │   └── RedisCacheManager.cs      # Redis key deletion
│   ├── Processors/
│   │   └── PurgeCacheProcessor.cs    # Core logic with cache key generation
│   └── Providers/
│       └── QueueClientProvider.cs    # Queue client factory
└── README.md                         # This file
```

## Testing

### Manual Testing

1. **Create a test message**:

   ```bash
   # Using Azure Storage Explorer or Azure CLI
   az storage message put \
     --queue-name purge-cache \
     --content '{"Host":"test.s2search.local"}' \
     --connection-string "..."
   ```

2. **Verify Redis keys before**:

   ```bash
   redis-cli KEYS "test.s2search.local*"
   ```

3. **Run the Cache Manager**:

   ```bash
   dotnet run
   ```

4. **Verify Redis keys after**:

   ```bash
   redis-cli KEYS "tests2searchlocal:*"
   # Should return empty list (note: colons are stripped from host)
   ```

5. **Check the logs**:
   The processor will log:
   - Connection test results
   - Number of messages processed
   - Cache keys being deleted
   - Any errors encountered

### Unit Testing

Create a test project:

```bash
dotnet new xunit -n S2Search.CacheManager.Tests
dotnet add reference ../S2Search.CacheManager/S2Search.CacheManager.csproj
```

Mock the dependencies using **Moq** or **NSubstitute**.

## Troubleshooting

### Common Issues

**❌ Unable to connect to Storage Account**

- Verify connection string format
- Check firewall rules on storage account
- Ensure queue `purge-cache` exists
- Test with Azure Storage Explorer

**❌ Unable to connect to Redis**

- Check Redis is running: `redis-cli ping`
- Verify connection string format (e.g., `localhost:6379` or `redis:6379`)
- Check network connectivity and firewall rules
- Ensure Redis accepts connections from your IP
- For Kubernetes deployments, verify service name matches Redis deployment

**❌ Messages not being deleted**

- Check message format (must be valid JSON)
- Verify Base64 encoding/decoding
- Look for exceptions in logs
- Check queue message visibility timeout

**❌ No messages being processed**

- Verify queue name is `purge-cache`
- Check queue has messages: `az storage message peek`
- Ensure correct storage account is configured
- Review logs for connection errors

### Logging

Enable detailed logging:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Services": "Trace",
      "Microsoft": "Information"
    }
  }
}
```

Log output includes:

- Connection test results
- Message count and IDs
- Cache keys being deleted
- Exception details with stack traces

## Contributing

1. **Fork** the repository
2. **Create a feature branch**: `git checkout -b feature/my-new-feature`
3. **Commit changes**: `git commit -am 'Add some feature'`
4. **Push to branch**: `git push origin feature/my-new-feature`
5. **Submit a pull request**

### Coding Guidelines

- Write unit tests for new functionality
- Update README for significant changes
- Follow existing code style and patterns
- Use meaningful commit messages
- Keep pull requests focused and small

---

## Related Projects

- [S2Search Backend API](../../APIs/S2Search.Backend/) - Main search API
- [S2Search Domain](../../Domain/S2Search.Backend.Domain/) - Shared domain models
- [S2Search Services](../../Services/S2Search.Backend.Services/) - Shared services layer

## License

Proprietary - Square2 Digital © 2025

## Support

For issues or questions:

- Create an issue in the repository
- Contact the development team at dev@square2digital.com
- Check internal documentation wiki
