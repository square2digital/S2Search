using S2Search.Backend.Domain.Customer.SearchResources.FeedCredentials;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

public interface IFeedCredentialsRepository
{
    Task<bool> CheckUserExists(Guid searchIndexId, string username);
    Task<FeedCredentials> GetCredentials(Guid searchIndexId);
}
