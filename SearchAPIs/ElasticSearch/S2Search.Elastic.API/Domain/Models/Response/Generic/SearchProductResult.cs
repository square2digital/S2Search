using Domain.Models.Facets;
using Domain.Models.Response.Vehicle;

namespace Domain.Models.Response.Generic
{
    public class SearchProductResult
    {
        public SearchProductResult()
        {
            GenericResults = new List<GenericResponse>();
            Results = new List<SearchVehicle>();
            Facets = new List<FacetGroup>();
        }

        public long TotalResults { get; set; }
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

        public IList<GenericResponse> GenericResults { get; set; }

        public IList<SearchVehicle> Results { get; set; }
    }
}
