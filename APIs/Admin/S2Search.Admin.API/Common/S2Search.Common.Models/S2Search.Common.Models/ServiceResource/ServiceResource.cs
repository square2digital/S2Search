using System;
using System.Collections.Generic;
using System.Text;

namespace S2Search.Common.Models.ServiceResource
{
    public class ServiceResource
    {
        public Guid ServiceResourceId { get; set; }
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
