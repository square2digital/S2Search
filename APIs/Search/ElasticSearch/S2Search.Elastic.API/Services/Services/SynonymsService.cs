﻿//using Domain.Constants;
using Domain.Interfaces;
//using Domain.Models.Interfaces;
using Domain.Models.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
//using S2Search.ClientConfigurationApi.Client.AutoRest;
using Services.Helpers;
using Services.Interfaces;
//using Services.Interfaces.Cache;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public class SynonymsService : ISynonymsService
    {
        private readonly IAppSettings _appSettings;
        //private readonly IClientConfigurationApiClient _clientConfigClient;
        //private readonly IDistributedCacheService _redisService;
        private readonly ILogger _logger;

        public SynonymsService(IAppSettings appSettings,
                               ILogger<SynonymsService> logger)
                               //IClientConfigurationApiClient clientConfigClient,
                               //IDistributedCacheService redisService)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            //_clientConfigClient = clientConfigClient ?? throw new ArgumentNullException(nameof(clientConfigClient));
            //_redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the Generic Synomyns from the Customer Resource DB
        /// </summary>
        /// <param name="category">Get Synomyns by category - default is "vehicles"</param>
        /// <returns></returns>
        public async Task<List<string>> GetGenericSynonyms(string category = "vehicles")
        {
            //string cacheKey = $"{CacheKeys.Synonyms}";
            //var redisKey = _redisService.CreateRedisKey(callingHost, cacheKey, HashHelper.GetXXHashString(cacheKey));
            //var redisValue = await _redisService.GetFromRedisIfExistsAsync(redisKey);

            //if (redisValue != null)
            //{
            //    var synonyms = JsonConvert.DeserializeObject<List<string>>(redisValue);
            //    return synonyms;
            //}

            var result = await GetGenericSynomynsFactory(category);
            //await _redisService.SetValueAsync(redisKey, JsonConvert.SerializeObject(result), CacheHelper.GetExpiry(_appSettings.RedisCacheSettings.DefaultCacheExpiryInSeconds));

            return result;
        }

        private async Task<List<string>> GetGenericSynomynsFactory(string category)
        {
            //_logger.LogInformation($"Cache miss on {nameof(GetGenericSynomynsFactory)}");

            //var header = ApiManagerHelper.GetHeader(_appSettings.ClientConfigurationSettings.HeaderAPISubscriptionName, _appSettings.ClientConfigurationSettings.APISubscriptionKey);
            //var response = await _clientConfigClient.GetGenericSynonymsWithHttpMessagesAsync(category, header);

            //if (!response.Response.IsSuccessStatusCode)
            //{
            //    if (response.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
            //    {
            //        return null;
            //    }
            //}

            var synonymsList = new List<string>();

            //var content = await response.Response.Content.ReadAsStringAsync();
            //var synonymsCollection = JsonConvert.DeserializeObject<IEnumerable<GenericSynonyms>>(content);

            //foreach (var synonym in synonymsCollection)
            //{
            //    synonymsList.Add(synonym.SolrFormat);
            //}

            return synonymsList;
        }
    }
}
