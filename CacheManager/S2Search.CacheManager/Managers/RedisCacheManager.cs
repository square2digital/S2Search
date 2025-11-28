using Microsoft.Extensions.Logging;
using S2Search.CacheManager.Interfaces.Managers;
using StackExchange.Redis;

namespace S2Search.CacheManager.Managers
{
    internal class RedisCacheManager : ICacheManager
    {
        private readonly ILogger logger;
        private readonly IDatabaseAsync redisDb;

        public RedisCacheManager(ILogger<RedisCacheManager> logger,
                                 IConnectionMultiplexer connectionMultiplexer)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (connectionMultiplexer is null)
            {
                throw new ArgumentNullException(nameof(connectionMultiplexer));
            }

            redisDb = connectionMultiplexer.GetDatabase();
        }

        public async Task DeleteKeysByWildcard(string keyRoot)
        {
            if (string.IsNullOrWhiteSpace(keyRoot))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(keyRoot));
            }

            var keyPattern = keyRoot.Trim().Last().ToString() == "*" ? keyRoot : $"{keyRoot}*";

            await foreach (var key in GetKeysByPatternAsync(keyPattern))
            {
                var deleted = await redisDb.KeyDeleteAsync(key);
                logger.LogInformation($"KeyPattern: {keyPattern} | Deleted: {deleted}");
            }
        }

        private async IAsyncEnumerable<string> GetKeysByPatternAsync(string keyPattern)
        {
            if (string.IsNullOrWhiteSpace(keyPattern))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(keyPattern));
            }

            foreach (var endpoint in redisDb.Multiplexer.GetEndPoints())
            {
                var server = redisDb.Multiplexer.GetServer(endpoint);
                await foreach (var key in server.KeysAsync(pattern: keyPattern))
                {
                    yield return key.ToString();
                }
            }
        }
    }
}
