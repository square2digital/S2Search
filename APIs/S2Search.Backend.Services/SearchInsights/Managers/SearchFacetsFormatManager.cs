using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;
using S2Search.Backend.Domain.Interfaces.SearchInsights.Managers;
using System.Globalization;

namespace S2Search.Backend.Services.SearchInsights.Managers
{
    public class SearchFacetsFormatManager : ISearchFacetsFormatManager
    {
        public IEnumerable<SearchFacet> GetSearchFacets(string unformattedFacets)
        {
            var listOfFacets = new List<SearchFacet>();

            if (string.IsNullOrEmpty(unformattedFacets))
            {
                return listOfFacets;
            }

            var facets = unformattedFacets.Split(',');
            var textInfo = CultureInfo.CurrentCulture.TextInfo;

            foreach (var facet in facets)
            {
                var expressionParts = facet.Split(' ');
                var category = expressionParts[0];
                var comparer = expressionParts[1];

                var isRange = comparer.Equals("ge") || comparer.Equals("le");

                if (isRange)
                {
                    var listOfExpressions = facet.Split(" and ").ToList();

                    if (listOfExpressions.Count != 2)
                    {
                        throw new InvalidOperationException($"Facet '{category}' is a range and does not contain 2 expressions");
                    }

                    var firstValue = listOfExpressions.First().Replace(category, "").Replace(" ge ", "");
                    var lastValue = listOfExpressions.Last().Replace(category, "").Replace(" le ", "");

                    listOfFacets.Add(new SearchFacet()
                    {
                        Category = textInfo.ToTitleCase(category),
                        Value = $"{firstValue}-{lastValue}"
                    });
                }
                else
                {
                    //equality check
                    var facetFormatted = facet.Replace(category, "").Replace(" eq ", "");

                    var facetValue = facetFormatted;

                    var isString = facetFormatted.Contains("'");

                    if (isString)
                    {
                        facetValue = facetFormatted.Replace("'", "");
                    }

                    listOfFacets.Add(new SearchFacet()
                    {
                        Category = textInfo.ToTitleCase(category),
                        Value = facetValue
                    });
                }
            }

            return listOfFacets;
        }
    }
}
