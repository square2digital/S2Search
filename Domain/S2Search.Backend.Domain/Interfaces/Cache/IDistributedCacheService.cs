using StackExchange.Redis;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;

public interface IDistributedCacheService
{
    string CreateRedisKey(string customerEndpoint, string endpointName, string requestHash);
    Task<bool> SetValueAsync(string key, string value, TimeSpan? expiry = null);
    Task<RedisValue> GetValueAsync(string key);
    Task<string> GetFromRedisIfExistsAsync(string redisKey);
}