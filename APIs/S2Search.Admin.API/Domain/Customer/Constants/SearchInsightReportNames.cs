using System.Collections.Generic;

namespace Domain.Constants
{
    public static class SearchInsightReportNames
    {
        public static readonly IReadOnlyList<string> Reports = new List<string>()
        {
            AllZeroResultSearches,
            PopularMakes,
            SearchesPerDay,
            Searches
        };


        public static readonly IReadOnlyDictionary<string, string[]> AllWithDataCategories = new Dictionary<string, string[]>()
        {
            { AllZeroResultSearches, new string[] { "Zero Results Text & Filter Searches", "Zero Results Text Searches", "Zero Results Filter Searches" } },
            { PopularMakes, new string[] { "Make" } }
        };

        public const string AllZeroResultSearches = nameof(AllZeroResultSearches);
        public const string PopularMakes = nameof(PopularMakes);
        public const string SearchesPerDay = nameof(SearchesPerDay);
        public const string Searches = nameof(Searches);
    }
}
