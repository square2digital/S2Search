using System.Collections.Generic;

namespace S2Search.Backend.Domain.Customer.Models;

public class SearchInsightSummary
{
    public IEnumerable<SearchInsightTile> Tiles { get; set; }
    public IEnumerable<SearchInsightChart> Charts { get; set; }
}
