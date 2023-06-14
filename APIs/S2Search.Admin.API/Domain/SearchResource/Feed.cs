using Domain.Customer.Enums;

namespace Domain.SearchResource
{
    public class Feed
    {
        public int FeedId { get; set; }
        public FeedType Type { get; set; }
        public string ScheduleCron { get; set; }
        public string ScheduleFriendlyDescription { get; set; }
    }
}
