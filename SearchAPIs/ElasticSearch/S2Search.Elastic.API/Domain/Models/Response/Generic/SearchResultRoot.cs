using Domain.Models.Insights;
using Domain.Models.Response.Vehicle;

namespace Domain.Models.Response.Generic
{
    public class SearchResultRoot
    {
        public SearchResultRoot()
        {
            SearchProductResult = new SearchProductResult();
            SearchInsightMessage = new SearchInsightMessage();
        }

        public SearchResultRoot(List<GenericResponse> documemts) : this()
        {
            SearchProductResult.GenericResults = documemts;
        }

        public SearchResultRoot(List<SearchVehicle> documemts) : this()
        {
            SearchProductResult.Results = documemts;
        }

        public SearchProductResult SearchProductResult { get; set; }
        public SearchInsightMessage SearchInsightMessage { get; set; }
    }
}
