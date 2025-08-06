using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;
using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Providers.Credentials
{
    public class SearchIndexQueryCredentialsProvider : ISearchIndexQueryCredentialsProvider
    {
        private readonly ILogger _logger;
        private readonly IAppSettings _appSettings;
        private readonly IDistributedCacheService _redisService;        

        public SearchIndexQueryCredentialsProvider(ILogger<SearchIndexQueryCredentialsProvider> logger,
                                                   IAppSettings appSettings,
                                                   IDistributedCacheService redisService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
        }

        public async Task<SearchIndexQueryCredentials> GetAsync(string callingHost)
        {
            try
            {
                if (_appSettings.DemoSearchCredentials.UseDemoSearchCredentials)
                {
                    return new SearchIndexQueryCredentials()
                    {
                        SearchIndexId = _appSettings.DemoSearchCredentials.SearchCredentialsIndexId,
                        QueryApiKey = _appSettings.DemoSearchCredentials.SearchCredentialsQueryKey,
                        SearchIndexName = _appSettings.DemoSearchCredentials.SearchCredentialsIndexName,
                        SearchInstanceEndpoint = _appSettings.DemoSearchCredentials.SearchCredentialsInstanceEndpoint,
                        SearchInstanceName = _appSettings.DemoSearchCredentials.SearchCredentialsInstanceName
                    };
                }

                string cacheKey = CacheKeys.QueryCredentials;
                var redisKey = _redisService.CreateRedisKey(cacheKey, callingHost, HashHelper.GetXXHashString(JsonConvert.SerializeObject(cacheKey)));
                var redisValue = await _redisService.GetFromRedisIfExistsAsync(redisKey);

                if (redisValue != null)
                {
                    var searchResults = JsonConvert.DeserializeObject<SearchIndexQueryCredentials>(redisValue);
                    return searchResults;
                }

                var result = await GetQueryCredentialsAsync(callingHost);

                if(result != null)
                {
                    await _redisService.SetValueAsync(redisKey, JsonConvert.SerializeObject(result), CacheHelper.GetExpiry(_appSettings.RedisCacheSettings.DefaultCacheExpiryInSeconds));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetAsync)} | CallingHost: {callingHost} | CacheDuration: {_appSettings.RedisCacheSettings.DefaultCacheExpiryInSeconds} | Message: {ex.Message}");
                throw;
            }
        }

        private async Task<SearchIndexQueryCredentials> GetQueryCredentialsAsync(string callingHost)
        {
            try
            {
                //var header = ApiManagerHelper.GetHeader(_appSettings.ClientConfigurationSettings.HeaderAPISubscriptionName, _appSettings.ClientConfigurationSettings.APISubscriptionKey);
                var response = await _clientConfigClient.GetSearchIndexQueryCredentialsAsync(callingHost);

                //if (!response.Response.IsSuccessStatusCode)
                //{
                //    if (response.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                //    {
                //        return null;
                //    }
                //}

                //var content = await response.Response.Content.ReadAsStringAsync();
                //var queryCredentials = JsonConvert.DeserializeObject<SearchIndexQueryCredentials>(content);
                var queryCredentials = response;
                var azureSearchResource = new SearchIndexQueryCredentials()
                {
                    SearchIndexId = queryCredentials.SearchIndexId,
                    SearchInstanceEndpoint = queryCredentials.SearchInstanceEndpoint,
                    SearchInstanceName = queryCredentials.SearchInstanceName,
                    SearchIndexName = queryCredentials.SearchIndexName,
                    QueryApiKey = queryCredentials.ApiKey
                };

                return azureSearchResource;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetQueryCredentialsAsync)} | CallingHost: {callingHost} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
