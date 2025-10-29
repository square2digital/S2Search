using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;

namespace Services.Interfaces.Repositories
{
    public interface IFeedServicesRepository
    {
        Task<string> GetDataFormatAsync(Guid customerId, string searchIndexName);
        Task<IEnumerable<string>> GetCurrentDocumentIdsAsync(Guid searchIndexId, int pageNumber, int pageSize);
        Task<int> GetCurrentDocumentsTotalAsync(Guid searchIndexId);
        Task MergeFeedDocumentsAsync(Guid searchIndexId, IEnumerable<NewFeedDocument> newFeedDocuments);
        Task<SearchIndexFeedProcessingData> GetSearchIndexFeedProcessingData(Guid customerId, string searchIndexName);

    }
}