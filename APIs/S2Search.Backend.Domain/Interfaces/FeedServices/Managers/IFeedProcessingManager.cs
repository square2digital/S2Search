using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;
using S2Search.Backend.Domain.AzureSearch.Indexes;
using Azure.Storage.Blobs;

namespace Services.Interfaces.Managers
{
    public interface IFeedProcessingManager
    {
        Task CreateOrUpdateIndexSuggesterAsync(SearchIndexCredentials searchIndexCredentials);
        Task ProcessFeedDataAsync(IEnumerable<VehicleIndex> feedData, SearchIndexCredentials searchIndexCredentials);
        Task UpdateFeedRepositoryAsync(IEnumerable<VehicleIndex> feedData, SearchIndexCredentials searchIndexCredentials);
        Task MoveCsvBlobAsync(BlobClient csvBlob, FeedBlob feedBlob);
        Task CreateOrUpdateIndexSynonymsAsync(SearchIndexCredentials searchIndexCredentials);
    }
}
