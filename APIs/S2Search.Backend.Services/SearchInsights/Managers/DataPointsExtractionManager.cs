using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;
using S2Search.Backend.Domain.Interfaces.SearchInsights.Managers;
using S2Search.Backend.Domain.Interfaces.SearchInsights.Providers;
using S2Search.Backend.Domain.AzureFunctions.SearchInsights;

namespace S2Search.Backend.Services.SearchInsights.Managers
{
    public class DataPointsExtractionManager : IDataPointsExtractionManager
    {
        private readonly IDateTimeCategoryProvider datetimeCategoryProvider;
        private readonly ISearchFacetsFormatManager searchFacetsFormatManager;

        public DataPointsExtractionManager(IDateTimeCategoryProvider datetimeCategoryProvider,
                                           ISearchFacetsFormatManager searchFacetsFormatManager)
        {
            this.datetimeCategoryProvider = datetimeCategoryProvider ?? throw new ArgumentNullException(nameof(datetimeCategoryProvider));
            this.searchFacetsFormatManager = searchFacetsFormatManager ?? throw new ArgumentNullException(nameof(searchFacetsFormatManager));
        }

        public IEnumerable<SearchInsightDataPoint> Extract(SearchInsightMessage searchInsightMessage)
        {
            var dataPoints = new List<SearchInsightDataPoint>();

            AddSearchTypeDataPoint(searchInsightMessage, dataPoints);
            AddSearchAndFiltersDataPoint(searchInsightMessage, dataPoints);
            AddDateBasedDataPoints(searchInsightMessage.DateGenerated, dataPoints);
            AddSearchFacetsDataPoints(searchInsightMessage, dataPoints);
            AddOrderByDataPoint(searchInsightMessage, dataPoints);

            return dataPoints;
        }

        private static void AddOrderByDataPoint(SearchInsightMessage searchInsightMessage, List<SearchInsightDataPoint> dataPoints)
        {
            var hasOrderBy = !string.IsNullOrEmpty(searchInsightMessage.OrderBy);
            var orderByDataPoint = hasOrderBy switch
            {
                true => searchInsightMessage.OrderBy,
                false => S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataPoints.WithoutOrderBy,
            };

            dataPoints.Add(new SearchInsightDataPoint()
            {
                DataCategory = S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.OrderBy,
                DataPoint = orderByDataPoint,
                Date = searchInsightMessage.DateGenerated
            });
        }

        private void AddSearchFacetsDataPoints(SearchInsightMessage searchInsightMessage, List<SearchInsightDataPoint> dataPoints)
        {
            var searchFacets = searchFacetsFormatManager.GetSearchFacets(searchInsightMessage.Filters);

            foreach (var facet in searchFacets)
            {
                dataPoints.Add(new SearchInsightDataPoint()
                {
                    DataCategory = facet.Category,
                    DataPoint = facet.Value,
                    Date = searchInsightMessage.DateGenerated
                });
            }
        }

        private void AddDateBasedDataPoints(DateTime dateGenerated, List<SearchInsightDataPoint> dataPoints)
        {
            //Examples: 1,2,3 .. 22, 23
            dataPoints.Add(new SearchInsightDataPoint()
            {
                DataCategory = S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.TimeOfDayByHour,
                DataPoint = $"{dateGenerated.Hour}",
                Date = dateGenerated
            });

            //Examples: Morning, Afternoon
            dataPoints.Add(new SearchInsightDataPoint()
            {
                DataCategory = S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.TimeOfDayByPart,
                DataPoint = datetimeCategoryProvider.GetPartOfDay(dateGenerated.TimeOfDay),
                Date = dateGenerated
            });

            //Examples: Monday, Tuesday
            dataPoints.Add(new SearchInsightDataPoint()
            {
                DataCategory = S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.DayOfWeek,
                DataPoint = $"{dateGenerated.DayOfWeek}",
                Date = dateGenerated
            });

            //Examples: Weekday, Weekend
            dataPoints.Add(new SearchInsightDataPoint()
            {
                DataCategory = S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.DayOfWeekPart,
                DataPoint = datetimeCategoryProvider.GetPartOfWeek(dateGenerated),
                Date = dateGenerated
            });

            //Examples: 1,2,3 .. 29,30,31
            dataPoints.Add(new SearchInsightDataPoint()
            {
                DataCategory = S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.DayOfMonth,
                DataPoint = $"{dateGenerated.Day}",
                Date = dateGenerated
            });

            //Examples: January, February, March
            dataPoints.Add(new SearchInsightDataPoint()
            {
                DataCategory = S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.MonthOfYear,
                DataPoint = $"{dateGenerated.ToString("MMMM")}",
                Date = dateGenerated
            });

            //Examples: Q1, Q2, Q3, Q4
            dataPoints.Add(new SearchInsightDataPoint()
            {
                DataCategory = S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.QuarterOfYear,
                DataPoint = datetimeCategoryProvider.GetQuarterOfYear(dateGenerated),
                Date = dateGenerated
            });
        }

        private void AddSearchAndFiltersDataPoint(SearchInsightMessage searchInsightMessage, List<SearchInsightDataPoint> dataPoints)
        {
            var isZeroResults = searchInsightMessage.TotalResults == 0;
            var isTextSearch = !string.IsNullOrEmpty(searchInsightMessage.ActualSearchQuery);
            var isFilerSearch = !string.IsNullOrEmpty(searchInsightMessage.Filters);

            var dataCategory = (isZeroResults, isTextSearch, isFilerSearch) switch
            {
                (false, true, true) => S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.SearchResultsWithTextAndFilters,
                (false, true, false) => S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.SearchResultsWithText,
                (false, false, true) => S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.SearchResultsWithFilters,
                (false, false, false) => "Results Without Text or Filters",
                (true, true, true) => S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.ZeroResultWithTextAndFilters,
                (true, true, false) => S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.ZeroResultWithText,
                (true, false, true) => S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.ZeroResultWithFilters,
                (true, false, false) => "Zero Results Without Text or Filters"
            };

            var dataPointText = dataCategory switch
            {
                S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.SearchResultsWithText => searchInsightMessage.ActualSearchQuery,
                S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.ZeroResultWithText => searchInsightMessage.ActualSearchQuery,
                S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.SearchResultsWithFilters => searchInsightMessage.Filters,
                S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.ZeroResultWithFilters => searchInsightMessage.Filters,
                S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.SearchResultsWithTextAndFilters => $"{searchInsightMessage.ActualSearchQuery} && {searchInsightMessage.Filters}",
                S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.ZeroResultWithTextAndFilters => $"{searchInsightMessage.ActualSearchQuery} && {searchInsightMessage.Filters}",
                _ => S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataPoints.WithoutTextOrFilters
            };

            dataPoints.Add(new SearchInsightDataPoint()
            {
                DataCategory = dataCategory,
                DataPoint = dataPointText,
                Date = searchInsightMessage.DateGenerated
            });
        }

        private void AddSearchTypeDataPoint(SearchInsightMessage searchInsightMessage, List<SearchInsightDataPoint> dataPoints)
        {
            var isTextSearch = !string.IsNullOrEmpty(searchInsightMessage.ActualSearchQuery);
            var isFilerSearch = !string.IsNullOrEmpty(searchInsightMessage.Filters);

            var searchTypeDataPoint = (isTextSearch, isFilerSearch) switch
            {
                (true, true) => S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataPoints.TextAndFilterSearches,
                (true, false) => S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataPoints.TextSearches,
                (false, true) => S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataPoints.FilterSearches,
                (false, false) => S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataPoints.WithoutTextOrFilters
            };

            dataPoints.Add(new SearchInsightDataPoint()
            {
                DataCategory = S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants.SearchInsights.DataCategories.SearchType,
                DataPoint = searchTypeDataPoint,
                Date = searchInsightMessage.DateGenerated
            });
        }
    }
}
