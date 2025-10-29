# S2Search Cache Manager

High-performance distributed caching layer for S2Search platform, providing Redis-based caching with intelligent cache management and multi-tier caching strategies.

## Overview

The Cache Manager is a critical component that dramatically improves S2Search performance by implementing intelligent caching strategies for:

- Search results and faceted queries
- Configuration data and themes
- User session management
- API response optimization
- Database query result caching

## Architecture

### Core Components

- **Redis Integration** - Primary distributed cache store
- **Multi-Tier Caching** - Memory + Redis hybrid approach
- **Cache Invalidation** - Smart invalidation strategies
- **Performance Monitoring** - Built-in metrics and health checks
- **Fallback Mechanisms** - Graceful degradation when cache unavailable

### Integration Points

The Cache Manager integrates with:

- [`S2Search.API`](../../APIs/S2Search.API) - Main backend caching
- [`ServiceCollectionExtension`](../../Services/S2Search.Backend.Services/ServiceCollectionExtension.cs) - Service registration
- **Search APIs** - Result caching for faster responses
- **Admin APIs** - Configuration caching

## Key Features

### 🚀 Performance Optimization

- **Sub-millisecond** response times for cached data
- **Intelligent Prefetching** - Predictive cache warming
- **Compression** - Optimized storage with compression
- **Connection Pooling** - Efficient Redis connection management

### 🛡️ Reliability

- **Circuit Breaker** - Automatic failover when Redis unavailable
- **Health Monitoring** - Continuous cache health assessment
- **Graceful Degradation** - Fallback to database when needed
- **Data Consistency** - Cache coherency mechanisms

### 📊 Analytics

- **Hit Rate Monitoring** - Cache effectiveness metrics
- **Performance Metrics** - Response time tracking
- **Usage Statistics** - Cache utilization analysis
- **Cost Optimization** - Memory usage optimization

## Technologies

- **.NET 8** - Modern framework with performance optimizations
- **Redis 6+** - Primary cache store with advanced features
- **StackExchange.Redis** - High-performance Redis client
- **Memory Cache** - L1 cache for ultra-fast access
- **JSON Serialization** - Efficient data serialization
- **Compression** - Data compression for storage optimization

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Redis 6+](https://redis.io/download) - Local or cloud instance
- [Docker](https://www.docker.com/) (optional for Redis container)

### Installation

1. **Install Redis (Docker):**

   ```bash
   docker run -d --name redis -p 6379:6379 redis:latest
   ```

2. **Clone and build:**

   ```bash
   git clone https://github.com/square2digital/S2Search.git
   cd S2Search/CacheManager/S2Search.CacheManager
   dotnet restore
   dotnet build
   ```

3. **Configuration:**
   Update connection strings in `appsettings.json`

### Basic Usage

```csharp
// Service registration
services.AddSingleton<ICacheManager, RedisCacheManager>();

// Usage in controllers/services
public class SearchController : ControllerBase
{
    private readonly ICacheManager _cache;

    public SearchController(ICacheManager cache)
    {
        _cache = cache;
    }

    public async Task<SearchResult> Search(string query)
    {
        var cacheKey = $"search:{query.ToLower()}";

        var cached = await _cache.GetAsync<SearchResult>(cacheKey);
        if (cached != null)
            return cached;

        var result = await _searchService.ExecuteSearch(query);
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(15));

        return result;
    }
}
```

## Configuration

### Redis Configuration

```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  },
  "CacheSettings": {
    "DefaultExpiration": "00:15:00",
    "MaxRetries": 3,
    "RetryDelay": "00:00:01",
    "CompressionEnabled": true,
    "HealthCheckInterval": "00:01:00"
  }
}
```

### Cache Policies

```csharp
public static class CachePolicies
{
    // Search results - 15 minutes
    public static readonly TimeSpan SearchResults = TimeSpan.FromMinutes(15);

    // Configuration data - 4 hours
    public static readonly TimeSpan Configuration = TimeSpan.FromHours(4);

    // User sessions - 24 hours
    public static readonly TimeSpan UserSessions = TimeSpan.FromHours(24);

    // Theme data - 1 hour
    public static readonly TimeSpan Themes = TimeSpan.FromHours(1);
}
```

## Cache Strategies

### Search Result Caching

```csharp
public class SearchCacheStrategy
{
    // Cache key patterns
    private const string SEARCH_KEY = "search:{hash}:{page}:{filters}";
    private const string FACETS_KEY = "facets:{hash}";
    private const string SUGGESTIONS_KEY = "suggest:{query}";

    // Intelligent cache warming
    public async Task WarmCache(SearchContext context)
    {
        // Pre-populate common searches
        var popularQueries = await GetPopularQueries();
        foreach (var query in popularQueries)
        {
            await ExecuteAndCacheSearch(query);
        }
    }
}
```

### Configuration Caching

```csharp
public class ConfigurationCacheStrategy
{
    // Cache configuration per customer
    private const string CONFIG_KEY = "config:{customerId}:{type}";

    // Invalidate when configuration changes
    public async Task InvalidateCustomerConfig(string customerId)
    {
        var pattern = $"config:{customerId}:*";
        await _cache.InvalidateByPatternAsync(pattern);
    }
}
```

## Performance Monitoring

### Health Checks

```csharp
public class CacheHealthCheck : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _cache.PingAsync();
            var stats = await _cache.GetStatsAsync();

            return HealthCheckResult.Healthy(
                $"Cache operational. Hit rate: {stats.HitRate:P2}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Cache unavailable", ex);
        }
    }
}
```

### Metrics Collection

```csharp
public class CacheMetrics
{
    private readonly IMetricsLogger _metrics;

    public void RecordCacheHit(string operation) =>
        _metrics.Counter("cache_hits").WithTag("operation", operation).Increment();

    public void RecordCacheMiss(string operation) =>
        _metrics.Counter("cache_misses").WithTag("operation", operation).Increment();

    public void RecordCacheLatency(string operation, TimeSpan duration) =>
        _metrics.Histogram("cache_latency").WithTag("operation", operation).Record(duration.TotalMilliseconds);
}
```

## Deployment

### Docker Support

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["S2Search.CacheManager.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "S2Search.CacheManager.dll"]
```

### Kubernetes Deployment

Integration with [`K8s/Legacy/K8s.Local.Development.Environment/redis`](../../K8s/Legacy/K8s.Local.Development.Environment/redis) for Redis deployment.

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: s2search-cache-manager
spec:
  replicas: 2
  selector:
    matchLabels:
      app: cache-manager
  template:
    metadata:
      labels:
        app: cache-manager
    spec:
      containers:
        - name: cache-manager
          image: s2search/cache-manager:latest
          env:
            - name: ConnectionStrings__Redis
              value: "redis:6379"
```

## Best Practices

### Cache Key Design

- Use hierarchical keys: `{service}:{entity}:{id}:{version}`
- Include relevant parameters in hash
- Consider key expiration strategies
- Use consistent naming conventions

### Memory Management

- Monitor memory usage regularly
- Implement cache size limits
- Use appropriate data structures
- Consider compression for large objects

### Error Handling

- Implement circuit breakers
- Provide fallback mechanisms
- Log cache failures appropriately
- Monitor cache health continuously

## Troubleshooting

### Common Issues

1. **High Memory Usage**

   ```bash
   # Monitor Redis memory
   redis-cli info memory

   # Check key distribution
   redis-cli --bigkeys
   ```

2. **Connection Issues**

   ```csharp
   // Implement retry logic
   services.Configure<RedisOptions>(options => {
       options.MaxRetries = 3;
       options.RetryDelay = TimeSpan.FromSeconds(1);
   });
   ```

3. **Performance Issues**
   ```bash
   # Monitor Redis performance
   redis-cli --latency-history -h localhost -p 6379
   ```

## Contributing

1. Follow .NET coding standards
2. Implement comprehensive logging
3. Add performance benchmarks
4. Include health checks for new features
5. Update documentation

See [CONTRIBUTING.md](../../CONTRIBUTING.md) for detailed guidelines.

## License

This project is proprietary software. See [LICENSE](../../LICENSE) for details.

---

_Built for enterprise-scale performance with Redis and .NET 8_
