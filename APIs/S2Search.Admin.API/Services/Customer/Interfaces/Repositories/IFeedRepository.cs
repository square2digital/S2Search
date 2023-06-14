using Domain.Customer.SearchResources.Feeds;

namespace Services.Customer.Interfaces.Repositories
{
    public interface IFeedRepository
    {
        Task<Feed> CreateAsync(FeedRequest feed);
        Task<Feed> GetLatestAsync(Guid searchIndexId);
    }
}
