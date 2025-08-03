using System;
using System.Collections.Generic;
using System.Text;

namespace S2Search.Common.Models.ServiceResource
{
    public class ServiceResourceCapacity
    {
        public int Id { get; set; }
        public Guid ServiceResourceId { get; set; }
        public decimal StorageQuotaMB { get; set; }
        public int IndexesQuota { get; set; }
        public decimal StorageUsedMB { get; set; }
        public int IndexesUsed { get; set; }
        public int DocumentsQuota { get; set; }
        public int DocumentsUsed { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
