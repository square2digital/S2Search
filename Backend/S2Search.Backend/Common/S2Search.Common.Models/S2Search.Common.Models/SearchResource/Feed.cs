using S2Search.Backend.Common.S2Search.Common.Models.S2Search.Common.Models.SearchResource.Enums;
using S2Search.Backend.Domain.Admin.SearchResource.Enums;
using S2Search.Backend.Domain.SearchResource.Enums;

namespace S2Search.Backend.Common.S2Search.Common.Models.S2Search.Common.Models.SearchResource
{
    public class Feed
    {
        public int FeedId { get; set; }
        public FeedType Type { get; set; }
        public string ScheduleCron { get; set; }
        public string ScheduleFriendlyDescription { get; set; }
    }
}
