using Services.Interfaces.Mappers;
using Services.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Providers
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
