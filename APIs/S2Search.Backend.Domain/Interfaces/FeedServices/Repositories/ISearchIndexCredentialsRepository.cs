using S2Search.Backend.Domain.AzureSearch.Indexes;
using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface ISearchIndexCredentialsRepository
    {
        Task<SearchIndexCredentials> GetCredentials(Guid customerId, string searchIndexName);
    }
}
