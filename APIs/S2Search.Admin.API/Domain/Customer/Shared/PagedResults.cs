using System.Collections.Generic;

namespace Domain.Customer.Shared
{
    public class PagedResults<T>
    {
        public IEnumerable<T> Results { get; set; }
        public int TotalCount { get; set; }
    }
}
