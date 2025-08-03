using S2Search.Common.Models.ServiceResource;
using System;
using System.Collections.Generic;
using System.Text;

namespace S2Search.Common.Models.SearchResource
{
    public class SearchResourceRequest
    {
        public Guid CustomerId { get; set; }
        public string IndexName { get; set; }
        public string IndexType { get; set; }
        public ServiceResourceRequest Configuration { get; set; }
    }
}
