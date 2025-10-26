namespace S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants
{
    public static class SearchInsights
    {
        public static class DataCategories
        {
            public const string SearchType = "Search Type";

            public const string SearchResultsWithText = "Text Searches";
            public const string SearchResultsWithFilters = "Filter Searches";
            public const string SearchResultsWithTextAndFilters = "Text & Filter Searches";

            public const string ZeroResultWithText = "Zero Results Text Searches";
            public const string ZeroResultWithFilters = "Zero Results Filter Searches";
            public const string ZeroResultWithTextAndFilters = "Zero Results Text & Filter Searches";

            public const string TimeOfDayByHour = "Time of Day (Hour)";
            public const string TimeOfDayByPart = "Time of Day (Part)";

            public const string DayOfWeek = "Day of Week (Day)";
            public const string DayOfWeekPart = "Day of Week (Part)";

            public const string DayOfMonth = "Day of Month (Day)";

            public const string MonthOfYear = "Month of Year";
            public const string QuarterOfYear = "Quarter Of Year";

            public const string OrderBy = "Order By";
        }

        public static class DataPoints
        {
            public const string TextSearches = "Text Searches";
            public const string FilterSearches = "Filter Searches";
            public const string TextAndFilterSearches = "Text & Filter Searches";
            public const string WithoutTextOrFilters = "No Text or Filter Searches";
            public const string WithoutOrderBy = "No Order By Selected";
        }
    }
}
