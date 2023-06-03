using System.Collections.Generic;

namespace Domain.Models
{
    public class SearchInsightSummary
    {
        public IEnumerable<SearchInsightTile> Tiles { get; set; }
        public IEnumerable<SearchInsightChart> Charts { get; set; }
    }
}
