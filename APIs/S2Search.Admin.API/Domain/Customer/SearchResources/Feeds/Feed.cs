using Domain.Customer.Enums;
using System;

namespace Domain.Customer.SearchResources.Feeds
{
    public class Feed
    {
        public int FeedId { get; set; }
        public FeedType Type { get; set; }
        public string ScheduleCron { get; set; }
        public Guid SearchIndexId { get; set; }
        public DateTime CreatedDate { get; set; }        
        public DateTime? SupersededDate { get; set; }
        public string ScheduleFriendlyDescription { get; set; }
        public bool IsLatest { get; set; }
    }
}
