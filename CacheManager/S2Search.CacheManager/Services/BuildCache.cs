using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S2Search.CacheManager.Services
{
    /// <summary>
    /// The process to build cache entries for S2Search.
    /// - using a set of all search facets from the index
    /// - create search combinations, such as Make + Model + Year + colour gradually increasing the complexity
    /// - for each combination, if the request has a non zero response save this to the cache
    /// - check to see of the generated cache key exists already, if so skip or delete and re-add
    /// - the combinations need to be ran in parallel to ensure high throughput
    /// </summary>
    public class BuildCache : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
