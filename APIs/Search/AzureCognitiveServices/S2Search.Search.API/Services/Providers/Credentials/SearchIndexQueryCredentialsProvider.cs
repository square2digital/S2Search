using Domain.Constants;
using Domain.Models.Interfaces;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Services.Helpers;
using Services.Interfaces.Cache;
using Domain.Configuration.SearchResources.Credentials;

namespace Services.Providers
{
    public class SearchIndexQueryCredentialsProvider : ISearchIndexQueryCredentialsProvider
    {
        private readonly ILogger _logger;
        private readonly IAdminService _adminService;
        private readonly IAppSettings _appSettings;
        private readonly IDistributedCacheService _redisService;        

        public SearchIndexQueryCredentialsProvider(ILogger<SearchIndexQueryCredentialsProvider> logger,
                                                   IAdminService clientConfigClient,
                                                   IAppSettings appSettings,
                                                   IDistributedCacheService redisService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _adminService = clientConfigClient ?? throw new ArgumentNullException(nameof(clientConfigClient));
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
                        ApiKey = _appSettings.DemoSearchCredentials.SearchCredentialsApiKey,
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
                var queryCredentials = await _adminService.GetSearchIndexQueryCredentials(callingHost);
                var azureSearchResource = new SearchIndexQueryCredentials()
                {
                    SearchIndexId = queryCredentials.SearchIndexId,
                    SearchInstanceEndpoint = queryCredentials.SearchInstanceEndpoint,
                    SearchInstanceName = queryCredentials.SearchInstanceName,
                    SearchIndexName = queryCredentials.SearchIndexName,
<<<<<<< HEAD:SearchAPIs/AzureCognitiveServices/S2Search.Search.API/Services/Providers/Credentials/SearchIndexQueryCredentialsProvider.cs
                    QueryApiKey = queryCredentials.ApiKey
=======
                    ApiKey = queryCredentials.ApiKey
>>>>>>> master:APIs/Search/AzureCognitiveServices/S2Search.Search.API/Services/Providers/Credentials/SearchIndexQueryCredentialsProvider.cs
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
