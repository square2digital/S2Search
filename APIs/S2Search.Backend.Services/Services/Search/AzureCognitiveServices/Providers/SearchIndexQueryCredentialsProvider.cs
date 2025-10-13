using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Domain.Interfaces.Repositories;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;
using Services.Interfaces;

namespace Services.Providers
{
    public class SearchIndexQueryCredentialsProvider : ISearchIndexQueryCredentialsProvider
    {
        private readonly ILogger _logger;
        private readonly ISearchIndexRepository _searchIndexRepo;
        private readonly IAppSettings _appSettings;
        private readonly IDistributedCacheService _redisService;

        public SearchIndexQueryCredentialsProvider(ILogger<SearchIndexQueryCredentialsProvider> logger,
                                                   ISearchIndexRepository searchIndexRepo,
                                                   IAppSettings appSettings,
                                                   IDistributedCacheService redisService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _searchIndexRepo = searchIndexRepo ?? throw new ArgumentNullException(nameof(searchIndexRepo));
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
                        id = _appSettings.DemoSearchCredentials.SearchCredentialsIndexId,
                        QueryApiKey = _appSettings.DemoSearchCredentials.SearchCredentialsQueryKey,
                        search_index_name = _appSettings.DemoSearchCredentials.SearchCredentialsIndexName,
                        search_instance_endpoint = _appSettings.DemoSearchCredentials.SearchCredentialsInstanceEndpoint,
                        search_instance_name = _appSettings.DemoSearchCredentials.SearchCredentialsInstanceName
                    };
                }

                if (_appSettings.RedisCacheSettings.EnableRedisCache)
                {
                    string cacheKey = CacheKeys.QueryCredentials;
                    var redisKey = _redisService.CreateRedisKey(cacheKey, callingHost, HashHelper.GetXXHashString(JsonConvert.SerializeObject(cacheKey)));
                    var redisValue = await _redisService.GetFromRedisIfExistsAsync(redisKey);

                    if (redisValue != null)
                    {
                        var searchResults = JsonConvert.DeserializeObject<SearchIndexQueryCredentials>(redisValue);
                        return searchResults;
                    }

                    var result = await GetQueryCredentialsAsync(callingHost);

                    if (result != null)
                    {
                        await _redisService.SetValueAsync(redisKey, JsonConvert.SerializeObject(result), CacheHelper.GetExpiry(_appSettings.RedisCacheSettings.DefaultCacheExpiryInSeconds));
                    }

                    return result;
                }
                else
                {
                    return await GetQueryCredentialsAsync(callingHost);
                }
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
                var queryCredentials = await _searchIndexRepo.GetQueryCredentials(callingHost);
                var azureSearchResource = new SearchIndexQueryCredentials()
                {
                    id = queryCredentials.id,
                    search_instance_endpoint = queryCredentials.search_instance_endpoint,
                    search_instance_name = queryCredentials.search_instance_name,
                    search_index_name = queryCredentials.search_index_name,
                    QueryApiKey = queryCredentials.api_key
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