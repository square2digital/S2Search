using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;
using StackExchange.Redis;
using System;
using System.Text;
using System.Threading.Tasks;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Services.Cache
{
    public class RedisService : IDistributedCacheService
    {
        private ConnectionMultiplexer _connection { get; set; }

        public RedisService(string connectionString)
        {
            _connection = ConnectionMultiplexer.Connect(connectionString);
        }

        private IDatabase GetDatabase
        {
            get
            {
                return _connection.GetDatabase();
            }        
        }

        public string CreateRedisKey(string firstKey, string secondKey, string requestHash)
        {
            StringBuilder sb = new StringBuilder();

            var formattedFirstKey = firstKey.Replace(":", string.Empty);
            var formattedSecondKey = secondKey.Replace(":", string.Empty);

            sb.Append(formattedFirstKey);
            sb.Append(':');
            sb.Append(formattedSecondKey);
            sb.Append(':');
            sb.Append(requestHash);

            return sb.ToString();
        }

        public async Task<bool> SetValueAsync(string key, string value, TimeSpan? expiry = null)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return await GetDatabase.StringSetAsync(key, value, expiry: expiry);
            }
            return false;
        }

        public async Task<RedisValue> GetValueAsync(string key)
        {
            return await GetDatabase.StringGetAsync(key);
        }

        public async Task<string> GetFromRedisIfExistsAsync(string redisKey)
        {
            try
            {
                var redisValue = await GetValueAsync(redisKey);
                if (redisValue.HasValue)
                {
                    return redisValue.ToString();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
