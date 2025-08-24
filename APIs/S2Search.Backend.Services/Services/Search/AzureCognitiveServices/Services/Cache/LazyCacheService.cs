using LazyCache;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;
using System;
using System.Threading.Tasks;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Services.Cache
{
    public class LazyCacheService : IMemoryCacheService
    {
        private readonly IAppCache _cache;

        public LazyCacheService(IAppCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public T GetOrAdd<T>(string cacheKey, Func<T> addItemFactory, TimeSpan slidingExpiry)
        {
            var result = _cache.GetOrAdd(cacheKey, addItemFactory, slidingExpiry);

            if (result == null)
            {
                _cache.Remove(cacheKey);
            }

            return result;
        }

        public async Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> addItemFactory)
        {
            var result = await _cache.GetOrAddAsync(cacheKey, addItemFactory);

            if (result == null)
            {
                _cache.Remove(cacheKey);
            }

            return result;
        }

        public async Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> addItemFactory, TimeSpan slidingExpiry)
        {
            var result = await _cache.GetOrAddAsync(cacheKey, addItemFactory, slidingExpiry);

            if(result == null)
            {
                _cache.Remove(cacheKey);
            }

            return result;
        }

        public async Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> addItemFactory, DateTimeOffset absoluteExpiry)
        {
            var result = await _cache.GetOrAddAsync(cacheKey, addItemFactory, absoluteExpiry);

            if (result == null)
            {
                _cache.Remove(cacheKey);
            }

            return result;
        }

        public async Task<T> GetAsync<T>(string cacheKey)
        {
            return await _cache.GetAsync<T>(cacheKey);
        }

        public T Get<T>(string cacheKey)
        {
            return _cache.Get<T>(cacheKey);
        }
    }
}
