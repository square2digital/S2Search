using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface IFeedRepository
    {
        Task<string> GetDataFormatAsync(Guid customerId, string searchIndexName);
        Task<IEnumerable<string>> GetCurrentDocumentIdsAsync(Guid searchIndexId, int pageNumber, int pageSize);
        Task<int> GetCurrentDocumentsTotalAsync(Guid searchIndexId);
        Task MergeFeedDocumentsAsync(Guid searchIndexId, IEnumerable<NewFeedDocument> newFeedDocuments);
        Task<SearchIndexFeedProcessingData> GetSearchIndexFeedProcessingData(Guid customerId, string searchIndexName);

    }
}