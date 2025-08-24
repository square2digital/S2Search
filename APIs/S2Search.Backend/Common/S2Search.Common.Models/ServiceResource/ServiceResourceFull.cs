using System;
using System.Collections.Generic;
using System.Text;

namespace S2Search.Backend.Common.S2Search.Common.Models.ServiceResource
{
    public class ServiceResourceFull
    {
        public ServiceResource ServiceResource { get; set; }
        public ServiceResourceCapacity ServiceResourceCapacity { get; set; }
        public IEnumerable<ServiceResourceKey> ServiceResourceKeys { get; set; }
    }
}
