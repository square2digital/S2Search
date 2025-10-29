using System.Collections.Generic;
using System.Linq;

namespace S2Search.Backend.Services.AzureFunctions.FeedServices.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int batchCount)
        {
            return items.Select((item, index) => new { item, index })
                        .GroupBy(x => x.index / batchCount)
                        .Select(g => g.Select(x => x.item));
        }
    }
}
