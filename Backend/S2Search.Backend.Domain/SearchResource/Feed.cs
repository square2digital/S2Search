using S2Search.Backend.Domain.SearchResource.Enums;

namespace S2Search.Backend.Domain.SearchResource
{
    public class Feed
    {
        public int FeedId { get; set; }
        public FeedType Type { get; set; }
        public string ScheduleCron { get; set; }
        public string ScheduleFriendlyDescription { get; set; }
    }
}
