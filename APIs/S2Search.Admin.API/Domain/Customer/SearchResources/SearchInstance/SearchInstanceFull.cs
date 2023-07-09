using System.Collections.Generic;

namespace Domain.Customer.SearchResources.SearchInstance
{
    public class SearchInstanceFull
    {
        public SearchInstance SearchInstance { get; set; }
        public SearchInstanceCapacity SearchInstanceCapacity { get; set; }
        public IEnumerable<SearchInstanceKey> SearchInstanceKeys { get; set; }
    }
}
