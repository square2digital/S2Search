using S2Search.Backend.Domain.Models.Facets;
using S2Search.Backend.Domain.Models.Objects;

namespace S2Search.Backend.Domain.Models.Response
{
    public class SearchProductResult
    {
        public int TotalResults { get; set; }
        public int SearchResults
        {
            get
            {
                if (Results != null)
                {
                    return Results.Count;
                }

                return 0;
            }
        }
        public IList<FacetGroup> Facets { get; set; }
        public IList<SearchVehicle> Results { get; set; }
    }
}
