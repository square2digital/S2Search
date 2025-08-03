using Azure;
using Domain.Configuration.SearchResources.Credentials;
using Domain.Models.Interfaces;
using Domain.Models.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAppSettings _appSettings;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public AdminService(IAppSettings appSettings,
            HttpClient httpClient,
            ILogger<AdminService> logger)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_appSettings.AdminSettings.AdminEndpoint);
        }

        /// <summary>
        /// - GetSearchIndexQueryCredentialsWithHttpMessagesAsync
        /// - - /api/QueryCredentials/endpoint/{customerEndpoint}
        /// </summary>
        /// <param name="customerEndpoint"></param>
        /// <returns></returns>
        public async Task<SearchIndexQueryCredentials> GetSearchIndexQueryCredentials(string customerEndpoint)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            try
            {
                var endpoint = $"/api/QueryCredentials/endpoint/{customerEndpoint}";
                httpResponseMessage = await _httpClient.GetAsync(endpoint);

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    throw new Exception($"Error on GetSearchIndexQueryCredentials - customerEndpoint {customerEndpoint}");
                }

                var json = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SearchIndexQueryCredentials>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Get Generic Synonyms
        /// - - /api/Configuration/search/GenericSynonyms/{category}
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<List<string>> GetGenericSynonyms(string category)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            try
            {
                var endpoint = $"/api/Configuration/search/GenericSynonyms/{category}";
                httpResponseMessage = await _httpClient.GetAsync(endpoint);


                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    throw new Exception($"Error on GetGenericSynonyms - category {category}");
                }

                var synonymsList = new List<string>();
                var json = await httpResponseMessage.Content.ReadAsStringAsync();
                var synonymsCollection = JsonConvert.DeserializeObject<IEnumerable<GenericSynonyms>>(json);

                foreach (var synonym in synonymsCollection)
                {
                    synonymsList.Add(synonym.SolrFormat);
                }

                return synonymsList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                throw;
            }
        }
    }
}
