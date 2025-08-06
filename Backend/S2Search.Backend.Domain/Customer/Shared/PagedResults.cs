using System.Collections.Generic;

namespace S2Search.Backend.Domain.Customer.Shared
{
    public class PagedResults<T>
    {
        public IEnumerable<T> Results { get; set; }
        public int TotalCount { get; set; }
    }
}
