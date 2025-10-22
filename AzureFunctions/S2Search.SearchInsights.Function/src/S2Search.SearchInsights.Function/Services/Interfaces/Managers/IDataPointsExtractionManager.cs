using Domain.Models;
using System.Collections.Generic;

namespace Services.Interfaces.Managers
{
    public interface IDataPointsExtractionManager
    {
        IEnumerable<SearchInsightDataPoint> Extract(SearchInsightMessage searchInsightMessage);
    }
}
