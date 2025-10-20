using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Managers
{
    public interface ISearchInsightsManager
    {
        Task SaveInsightsAsync(Guid searchIndexId, IEnumerable<SearchInsightDataPoint> dataPoints, DateTime dateGenerated);
    }
}
