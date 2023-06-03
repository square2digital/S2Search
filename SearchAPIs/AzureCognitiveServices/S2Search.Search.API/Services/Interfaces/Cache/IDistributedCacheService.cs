using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Cache
{
    public interface IDistributedCacheService
    {
        string CreateRedisKey(string callingHost, string endpointName, string requestHash);
        Task<bool> SetValueAsync(string key, string value, TimeSpan? expiry = null);
        Task<RedisValue> GetValueAsync(string key);
        Task<string> GetFromRedisIfExistsAsync(string redisKey);
    }
}