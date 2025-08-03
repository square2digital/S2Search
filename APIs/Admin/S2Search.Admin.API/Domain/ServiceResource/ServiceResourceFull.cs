using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ServiceResource
{
    public class ServiceResourceFull
    {
        public ServiceResource ServiceResource { get; set; }
        public ServiceResourceCapacity ServiceResourceCapacity { get; set; }
        public IEnumerable<ServiceResourceKey> ServiceResourceKeys { get; set; }
    }
}
