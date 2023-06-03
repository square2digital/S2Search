using Domain.SearchResources.SearchIndex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Customer
{
    public class CustomerFull
    {
        public Customer Customer { get; set; }
        public IEnumerable<SearchIndex> SearchIndexes { get; set; }
    }
}
