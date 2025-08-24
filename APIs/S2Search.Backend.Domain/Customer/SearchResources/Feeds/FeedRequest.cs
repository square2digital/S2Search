using System;

namespace S2Search.Backend.Domain.Customer.SearchResources.Feeds;

public class FeedRequest
{
    public Guid SearchIndexId { get; set; }
    public string FeedType { get; set; }
    public int ScheduleMinutes { get; set; }
}
