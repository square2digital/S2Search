using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;

namespace Services.Services
{
    public class SynonymsService : ISynonymsService
    {
        private readonly IAppSettings _appSettings;
        private readonly IDistributedCacheService _redisService;
        private readonly ILogger _logger;

        public SynonymsService(IAppSettings appSettings,
                               ILogger<SynonymsService> logger,
                               IDistributedCacheService redisService)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the Generic Synomyns from the Customer Resource DB
        /// </summary>
        /// <param name="category">Get Synomyns by category - default is "vehicles"</param>
        /// <returns></returns>
        public async Task<List<string>> GetGenericSynonyms(string callingHost, string category = "vehicles")
        {
            string cacheKey = $"{CacheKeys.Synonyms}";
            var redisKey = _redisService.CreateRedisKey(callingHost, cacheKey, S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers.HashHelper.GetXXHashString(cacheKey));
            var redisValue = await _redisService.GetFromRedisIfExistsAsync(redisKey);

            if (redisValue != null)
            {
                var synonyms = JsonConvert.DeserializeObject<List<string>>(redisValue);
                return synonyms;
            }

            var result = await GetGenericSynomynsFactory(category);
            await _redisService.SetValueAsync(redisKey, JsonConvert.SerializeObject(result), CacheHelper.GetExpiry(_appSettings.RedisCacheSettings.DefaultCacheExpiryInSeconds));

            return result;
        }

        private async Task<List<string>> GetGenericSynomynsFactory(string category)
        {
            //_logger.LogInformation($"Cache miss on {nameof(GetGenericSynomynsFactory)}");

            //var header = ApiManagerHelper.GetHeader(_appSettings.ClientConfigurationSettings.HeaderAPISubscriptionName, _appSettings.ClientConfigurationSettings.APISubscriptionKey);
            //var response = await _clientConfigClient.GetGenericSynonymsAsync(category);

            var synonymsList = new List<string>();

            //var synonymsCollection = response;

            //foreach (var synonym in synonymsCollection)
            //{
            //    synonymsList.Add(synonym.SolrFormat);
            //}

            return synonymsList;
        }
    }
}
