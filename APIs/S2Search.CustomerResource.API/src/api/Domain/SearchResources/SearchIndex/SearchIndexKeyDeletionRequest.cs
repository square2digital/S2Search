using Domain.Shared;
using System;
using System.Collections.Generic;

namespace Domain.SearchResources.SearchIndex
{
    public class SearchIndexKeyDeletionRequest
    {
        public Guid SearchIndexId { get; set; }
        public Guid CustomerId { get; set; }
        public IEnumerable<QueryKey> KeysToDelete { get; set; }
    }
}
