using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Rest.Azure;
using Services.Extensions;
using Services.Interfaces.Managers;
using Services.Interfaces.Providers;

namespace Services.Managers
{
    public class AzureSearchManager : IAzureSearchManager
    {
        private readonly IAzureSearchDocumentsClientProvider _clientProvider;
        private readonly ILogger _logger;
        private const string _newLineSeperator = "\n";
        private const int _enableSynonymsMaxTries = 3;
        private const int _azureRecommendedBatchSize = 1000;

        public AzureSearchManager(IAzureSearchDocumentsClientProvider clientProvider,
                                  ILogger<AzureSearchManager> logger)
        {
            _clientProvider = clientProvider ?? throw new ArgumentNullException(nameof(clientProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task CreateOrUpdateIndexAsync<T>(SearchIndexCredentials searchIndexCredentials)
        {
            try
            {
                SearchIndex indexDefinition = BuildSearchIndexFromType<T>(searchIndexCredentials.SearchIndexName);

                await _clientProvider.GetIndexClient(searchIndexCredentials.Endpoint,
                                                      searchIndexCredentials.SearchIndexName,
                                                      searchIndexCredentials.ApiKey).CreateOrUpdateIndexAsync(indexDefinition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(CreateOrUpdateIndexAsync)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task CreateOrUpdateIndexWithSuggesterAsync<T>(SearchIndexCredentials searchIndexCredentials, string suggesterName, IEnumerable<string> suggestFields)
        {
            try
            {
                SearchIndex searchIndex = BuildSearchIndexFromType<T>(searchIndexCredentials.SearchIndexName);

                var suggester = new SearchSuggester(suggesterName, suggestFields);
                searchIndex.Suggesters.Add(suggester);

                await _clientProvider.GetIndexClient(searchIndexCredentials.Endpoint,
                                                      searchIndexCredentials.SearchIndexName,
                                                      searchIndexCredentials.ApiKey).CreateOrUpdateIndexAsync(searchIndex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(CreateOrUpdateIndexWithSuggesterAsync)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task CreateOrUpdateSynonymMapAsync(SearchIndexCredentials searchIndexCredentials, string mapName, IEnumerable<string> synonyms)
        {
            try
            {
                var synonymsBuilder = new StringBuilder();
                synonymsBuilder.AppendJoin(_newLineSeperator, synonyms);

                var synonymMap = new SynonymMap(mapName, synonymsBuilder.ToString());

                await _clientProvider.GetIndexClient(searchIndexCredentials.Endpoint,
                                                      searchIndexCredentials.SearchIndexName,
                                                      searchIndexCredentials.ApiKey).CreateOrUpdateSynonymMapAsync(synonymMap);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(CreateOrUpdateSynonymMapAsync)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task MergeOrUploadDocumentsAsync<T>(IEnumerable<T> documents, SearchIndexCredentials searchIndexCredentials)
        {
            try
            {
                var documentBatches = documents.Batch(_azureRecommendedBatchSize);
                var searchClient = _clientProvider.GetSearchClient(searchIndexCredentials.Endpoint,
                                                                   searchIndexCredentials.SearchIndexName,
                                                                   searchIndexCredentials.ApiKey);

                foreach (var documentBatch in documentBatches)
                {
                    var options = new IndexDocumentsOptions { ThrowOnAnyError = true };

                    await searchClient.MergeOrUploadDocumentsAsync(documentBatch, options);
                }
            }
            catch (RequestFailedException ex)
            {
                // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                // the batch. Depending on your application, you can take compensating actions like delaying and
                // retrying. For this simple demo, we just log the failed document keys and continue.
                //ConsoleHelper.WriteErrorMessage($"Failed to index some of the documents: {String.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key))}");

                _logger.LogWarning(ex, $"Warning on {nameof(MergeOrUploadDocumentsAsync)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteDocumentsByIdsAsync(string keyName, IEnumerable<string> idsToDelete, SearchIndexCredentials searchIndexCredentials)
        {
            if (!idsToDelete.Any())
            {
                return;
            }

            try
            {
                var idBatches = idsToDelete.Batch(_azureRecommendedBatchSize);
                var searchClient = _clientProvider.GetSearchClient(searchIndexCredentials.Endpoint,
                                                                   searchIndexCredentials.SearchIndexName,
                                                                   searchIndexCredentials.ApiKey);

                foreach (var ids in idBatches)
                {
                    var options = new IndexDocumentsOptions { ThrowOnAnyError = true };

                    await searchClient.DeleteDocumentsAsync(keyName, ids, options);
                }
            }
            catch (RequestFailedException ex)
            {
                // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                // the batch. Depending on your application, you can take compensating actions like delaying and
                // retrying. For this simple demo, we just log the failed document keys and continue.
                //ConsoleHelper.WriteErrorMessage($"Failed to index some of the documents: {String.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key))}");

                _logger.LogWarning(ex, $"Warning on {nameof(DeleteDocumentsByIdsAsync)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task ApplySynonynMapToIndexAsync(SearchIndexCredentials searchIndexCredentials, string mapName, IEnumerable<string> targetFields)
        {
            try
            {
                var indexClient = _clientProvider.GetIndexClient(searchIndexCredentials.Endpoint,
                                               searchIndexCredentials.SearchIndexName,
                                               searchIndexCredentials.ApiKey);

                var targetIndex = await indexClient.GetIndexAsync(searchIndexCredentials.SearchIndexName);
                var searchIndex = targetIndex.Value;

                if (!searchIndex.Fields.Any(x => targetFields.Any(y => string.Equals(y, x.Name, StringComparison.OrdinalIgnoreCase))))
                {
                    throw new Exception($"SearchIndex '{searchIndex.Name}' does not contain all of the targetFields");
                }

                foreach (var field in targetFields)
                {
                    var hasSynonymMap = searchIndex.Fields.First(x => x.Name == field).SynonymMapNames.Contains(mapName);

                    if (hasSynonymMap)
                    {
                        continue;
                    }

                    searchIndex.Fields.First(x => x.Name == field).SynonymMapNames.Add(mapName);
                }

                await EnableSynoynmsSafetly(indexClient, searchIndex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(ApplySynonynMapToIndexAsync)} | Message: {ex.Message}");
                throw;
            }
        }

        private async Task EnableSynoynmsSafetly(SearchIndexClient indexClient, SearchIndex searchIndex)
        {
            for (int i = 0; i < _enableSynonymsMaxTries; ++i)
            {
                try
                {
                    await indexClient.CreateOrUpdateIndexAsync(searchIndex);

                    break;
                }
                catch (CloudException cloudEx)
                {
                    _logger.LogWarning(cloudEx, $"Warning {nameof(EnableSynoynmsSafetly)} failed | Attempt({i}/{_enableSynonymsMaxTries}). | Message: {cloudEx.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error on {nameof(EnableSynoynmsSafetly)} | Message: {ex.Message}");
                }
            }
        }

        private static SearchIndex BuildSearchIndexFromType<T>(string searchIndexName)
        {
            return new SearchIndex(searchIndexName)
            {
                Fields = new FieldBuilder().Build(typeof(T))
            };
        }
    }
}
