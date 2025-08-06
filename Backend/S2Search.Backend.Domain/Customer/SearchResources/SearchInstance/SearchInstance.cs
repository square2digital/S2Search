using System;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchInstance
{
    public class SearchInstance
    {
        public Guid SearchInstanceId { get; set; }
        public string Name { get; set; }
        public Guid SubscriptionId { get; set; }
        public string ResourceGroup { get; set; }
        public string Location { get; set; }
        public string PricingTier { get; set; }
        public int Replicas { get; set; }
        public int Partitions { get; set; }
        public bool IsShared { get; set; }

    }
}
