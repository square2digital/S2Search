using Services.Interfaces.Mappers;
using Services.Interfaces.Providers;

namespace S2Search.Backend.Services.AzureFunctions.FeedServices.Providers
{
    public class FeedMapperProvider : IFeedMapperProvider
    {
        private readonly IEnumerable<IFeedMapper> feedMappers;

        public FeedMapperProvider(IEnumerable<IFeedMapper> feedMappers)
        {
            this.feedMappers = feedMappers ?? throw new ArgumentNullException(nameof(feedMappers));
        }

        public IFeedMapper GetMapper(string feedDataFormat)
        {
            var feedMapper = feedMappers.FirstOrDefault(x => x.FeedDataFormat == feedDataFormat);

            if(feedMapper is null)
            {
                throw new NotImplementedException();
            }

            return feedMapper;
        }
    }
}
