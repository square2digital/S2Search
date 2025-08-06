using System;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchInstance
{
    public class SearchInstanceCapacity
    {
        public int Id { get; set; }
        public Guid SearchInstanceId { get; set; }
        public decimal StorageQuotaMB { get; set; }
        public int IndexesQuota { get; set; }
        public decimal StorageUsedMB { get; set; }
        public int IndexesUsed { get; set; }
        public int DocumentsQuota { get; set; }
        public int DocumentsUsed { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
