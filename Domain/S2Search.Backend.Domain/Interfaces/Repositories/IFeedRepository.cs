using S2Search.Backend.Domain.Customer.SearchResources.Feeds;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

public interface IFeedRepository
{
    Task<Feed> CreateAsync(FeedRequest feed);
    Task<Feed> GetLatestAsync(Guid searchIndexId);
}
