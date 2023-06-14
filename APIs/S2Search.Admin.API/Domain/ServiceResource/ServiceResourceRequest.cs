using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ServiceResource
{
    public class ServiceResourceRequest
    {
        public string Location { get; set; }
        public string PricingTier { get; set; }
        public bool IsShared { get; set; }
    }
}
