using Domain.SearchResources.Feeds;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface IFeedRepository
    {
        Task<Feed> CreateAsync(FeedRequest feed);
        Task<Feed> GetLatestAsync(Guid searchIndexId);
    }
}
