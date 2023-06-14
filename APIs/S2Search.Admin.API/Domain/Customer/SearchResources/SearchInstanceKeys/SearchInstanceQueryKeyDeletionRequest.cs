using Domain.Shared;
using System;
using System.Collections.Generic;

namespace Domain.SearchResources.SearchInstanceKeys
{
    public class SearchInstanceQueryKeyDeletionRequest
    {
        public Guid SearchIndexId { get; set; }
        public Guid SearchInstanceId { get; set; }
        public IEnumerable<QueryKey> KeysToDelete { get; set; }
    }
}
