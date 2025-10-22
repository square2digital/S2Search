using Domain.AzureSearch.Index;
using Domain.Models;
using Microsoft.Azure.Storage.Blob;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Managers
{
    public interface IFeedProcessingManager
    {
        Task CreateOrUpdateIndexSuggesterAsync(SearchIndexCredentials searchIndexCredentials);
        Task ProcessFeedDataAsync(IEnumerable<VehicleIndex> feedData, SearchIndexCredentials searchIndexCredentials);
        Task UpdateFeedRepositoryAsync(IEnumerable<VehicleIndex> feedData, SearchIndexCredentials searchIndexCredentials);
        Task MoveCsvBlobAsync(CloudBlockBlob csvBlob, FeedBlob feedBlob);
        Task CreateOrUpdateIndexSynonymsAsync(SearchIndexCredentials searchIndexCredentials);
    }
}
