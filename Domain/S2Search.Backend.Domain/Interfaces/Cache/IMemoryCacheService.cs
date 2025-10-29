using System;
using System.Threading.Tasks;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;

public interface IMemoryCacheService
{
    /// <summary>
    /// Gets or adds to the cache with a sliding expiry.
    /// If the client factory returns null this will NOT be stored in the cache.
    /// </summary>
    T GetOrAdd<T>(string cacheKey, Func<T> addItemFactory, TimeSpan slidingExpiry);

    /// <summary>
    /// Gets or adds to the cache with no expiry.
    /// If the client factory returns null this will NOT be stored in the cache.
    /// </summary>
    Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> addItemFactory);
    /// <summary>
    /// Gets or adds to the cache with a sliding expiry.
    /// If the client factory returns null this will NOT be stored in the cache.
    /// </summary>
    Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> addItemFactory, TimeSpan slidingExpiry);

    /// <summary>
    /// Gets or adds to the cache with an absolute expiry.
    /// If the client factory returns null this will NOT be stored in the cache.
    /// </summary>
    Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> addItemFactory, DateTimeOffset absoluteExpiry);

    T Get<T>(string cacheKey);

    Task<T> GetAsync<T>(string cacheKey);
}
