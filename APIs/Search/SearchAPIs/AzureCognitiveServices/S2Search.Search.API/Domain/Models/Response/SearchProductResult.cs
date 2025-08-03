using Domain.Models.Facets;
using Domain.Models.Objects;
using System.Collections.Generic;

namespace Domain.Models.Response
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
