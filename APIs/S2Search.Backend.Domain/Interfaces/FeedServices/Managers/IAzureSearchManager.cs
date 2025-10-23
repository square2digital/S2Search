using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;

namespace Services.Interfaces.Managers
{
    public interface IAzureSearchManager
    {
        Task CreateOrUpdateIndexAsync<T>(SearchIndexCredentials searchIndexCredentials);
        Task CreateOrUpdateIndexWithSuggesterAsync<T>(SearchIndexCredentials searchIndexCredentials, string suggesterName, IEnumerable<string> suggestFields);
        Task CreateOrUpdateSynonymMapAsync(SearchIndexCredentials searchIndexCredentials, string mapName, IEnumerable<string> synonyms);
        Task ApplySynonynMapToIndexAsync(SearchIndexCredentials searchIndexCredentials, string mapName, IEnumerable<string> targetFields);
        Task MergeOrUploadDocumentsAsync<T>(IEnumerable<T> documents, SearchIndexCredentials searchIndexCredentials);
        Task DeleteDocumentsByIdsAsync(string keyName, IEnumerable<string> ids, SearchIndexCredentials searchIndexCredentials);
    }
}
