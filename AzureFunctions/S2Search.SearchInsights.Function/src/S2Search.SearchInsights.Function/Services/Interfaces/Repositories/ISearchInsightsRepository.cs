using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface ISearchInsightsRepository
    {
        Task SaveInsightsAsync(Guid searchIndexId, IEnumerable<SearchInsightDataPoint> dataPoints);
        Task SaveSearchRequestAsync(Guid searchIndexId, DateTime date);
    }
}
