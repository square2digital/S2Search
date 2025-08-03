using Domain.SearchResources.FeedCredentials;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface IFeedCredentialsRepository
    {
        Task<bool> CheckUserExists(Guid searchIndexId, string username);
        Task<FeedCredentials> GetCredentials(Guid searchIndexId);
    }
}
