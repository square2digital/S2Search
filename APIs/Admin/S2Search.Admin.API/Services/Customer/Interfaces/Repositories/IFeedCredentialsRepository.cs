using Domain.Customer.SearchResources.FeedCredentials;

namespace Services.Customer.Interfaces.Repositories
{
    public interface IFeedCredentialsRepository
    {
        Task<bool> CheckUserExists(Guid searchIndexId, string username);
        Task<FeedCredentials> GetCredentials(Guid searchIndexId);
    }
}
