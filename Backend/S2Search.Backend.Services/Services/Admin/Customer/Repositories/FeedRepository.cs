using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.SearchResources.Feeds;
using S2Search.Backend.Services.Admin.Customer.Interfaces.Managers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Repositories
{
    public class FeedRepository : IFeedRepository
    {
        private readonly IDbContextProvider _dbContext;
        private readonly IFeedSettingsValidationManager _feedValidation;
        private readonly ICronDescriptionManager _cronDescriber;

        public FeedRepository(IDbContextProvider dbContext,
                              IFeedSettingsValidationManager feedValidation,
                              ICronDescriptionManager cronDescriber)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _feedValidation = feedValidation ?? throw new ArgumentNullException(nameof(feedValidation));
            _cronDescriber = cronDescriber ?? throw new ArgumentNullException(nameof(cronDescriber));
        }

        public async Task<Feed> CreateAsync(FeedRequest feed)
        {
            return await SaveFeedAsync(feed);
        }

        public async Task<Feed> GetLatestAsync(Guid searchIndexId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<Feed>(ConnectionStrings.CustomerResourceStore, StoredProcedures.GetLatestFeed, parameters);

            if (result != null)
            {
                result.ScheduleFriendlyDescription = _cronDescriber.Get(result.ScheduleCron);
            }

            return result;
        }

        private async Task<Feed> SaveFeedAsync(FeedRequest feed)
        {
            if (!_feedValidation.IsValid(feed, out string errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            string cronExpression = ConvertToCronExpression(feed);

            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", feed.SearchIndexId },
                { "FeedType", feed.FeedType },
                { "FeedCron", cronExpression }
            };

            await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore, StoredProcedures.AddFeed, parameters);

            var result = await GetLatestAsync(feed.SearchIndexId);
            return result;
        }

        private static string ConvertToCronExpression(FeedRequest feed)
        {
            return $"0 */{feed.ScheduleMinutes} * ? * *";
        }
    }
}
