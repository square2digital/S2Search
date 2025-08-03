using S2Search.Common.Models.SearchResource.Enums;

namespace S2Search.Common.Models.SearchResource
{
    public class Feed
    {
        public int FeedId { get; set; }
        public FeedType Type { get; set; }
        public string ScheduleCron { get; set; }
        public string ScheduleFriendlyDescription { get; set; }
    }
}
