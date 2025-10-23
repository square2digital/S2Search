using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.Models;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Services.Services.Admin.Customer.Managers
{
    public class SearchInsightsRetrievalManager : ISearchInsightsRetrievalManager
    {
        private readonly ISearchInsightsRepository searchInsightsRepo;
        private readonly IPreviousDateRangeProvider previousDateRangeProvider;
        private readonly IPercentageChangeProvider percentageChangeProvider;
        private readonly ISearchInsightFriendlyNameProvider searchInsightFriendlyNameProvider;
        private readonly ISearchInsightsReportRepository searchInsightsReportRepository;

        public SearchInsightsRetrievalManager(ISearchInsightsRepository searchInsightsRepo,
                                              IPreviousDateRangeProvider previousDateRangeProvider,
                                              IPercentageChangeProvider percentageChangeProvider,
                                              ISearchInsightFriendlyNameProvider searchInsightFriendlyNameProvider,
                                              ISearchInsightsReportRepository searchInsightsReportRepository)
        {
            this.searchInsightsRepo = searchInsightsRepo ?? throw new ArgumentNullException(nameof(searchInsightsRepo));
            this.previousDateRangeProvider = previousDateRangeProvider ?? throw new ArgumentNullException(nameof(previousDateRangeProvider));
            this.percentageChangeProvider = percentageChangeProvider ?? throw new ArgumentNullException(nameof(percentageChangeProvider));
            this.searchInsightFriendlyNameProvider = searchInsightFriendlyNameProvider ?? throw new ArgumentNullException(nameof(searchInsightFriendlyNameProvider));
            this.searchInsightsReportRepository = searchInsightsReportRepository ?? throw new ArgumentNullException(nameof(searchInsightsReportRepository));
        }

        public async Task<SearchInsightChart> GetChartByNameAsync(Guid searchIndexId,
                                                                  DateTime dateFrom,
                                                                  DateTime dateTo,
                                                                  string reportName)
        {
            var reportExists = searchInsightsReportRepository.Exists(reportName);

            if (!reportExists)
            {
                return null;
            }

            var dataCategories = TryGetDataCategories(reportName);
            var hasDataCategories = dataCategories?.Length > 0;
            var friendlyName = searchInsightFriendlyNameProvider.Get(reportName, dateFrom, dateTo);
            IEnumerable<SearchInsight> results;

            if (!hasDataCategories)
            {
                results = await searchInsightsRepo.GetCountAsync(searchIndexId, dateFrom, dateTo);
            }
            else
            {
                var allResults = await searchInsightsRepo.GetByCategoriesAsync(searchIndexId,
                                                                               dateFrom,
                                                                               dateTo,
                                                                               string.Join(",", dataCategories));

                results = GroupResults(allResults);
            }

            results = OrderResults(reportName, results);

            var chart = new SearchInsightChart()
            {
                Title = friendlyName,
                Data = results
            };

            return chart;
        }

        private IEnumerable<SearchInsight> OrderResults(string reportName, IEnumerable<SearchInsight> results)
        {
            results = reportName switch
            {
                SearchInsightReportNames.SearchesPerDay => results.OrderBy(x => x.Date),
                SearchInsightReportNames.PopularMakes => results.OrderByDescending(x => x.Count),
                _ => results,
            };

            return results;
        }

        public async Task<SearchInsightTile> GetTileByNameAsync(Guid searchIndexId,
                                                                DateTime dateFrom,
                                                                DateTime dateTo,
                                                                string reportName,
                                                                bool includePreviousPeriod)
        {
            var reportExists = searchInsightsReportRepository.Exists(reportName);

            if (!reportExists)
            {
                return null;
            }

            var dataCategories = TryGetDataCategories(reportName);
            var hasDataCategories = dataCategories?.Length > 0;
            var friendlyName = searchInsightFriendlyNameProvider.Get(reportName, dateFrom, dateTo);
            IEnumerable<SearchInsight> results;

            if (!hasDataCategories)
            {
                results = await searchInsightsRepo.GetCountAsync(searchIndexId, dateFrom, dateTo);
            }
            else
            {
                results = await searchInsightsRepo.GetByCategoriesAsync(searchIndexId,
                                                                               dateFrom,
                                                                               dateTo,
                                                                               string.Join(",", dataCategories));
            }

            var tile = new SearchInsightTile()
            {
                Title = friendlyName,
                Count = results.Sum(x => x.Count)
            };

            if (includePreviousPeriod)
            {
                var (previousDateFrom, previousDateTo, daysDifference) = previousDateRangeProvider.Get(dateFrom, dateTo);

                IEnumerable<SearchInsight> previousPeriodResults;

                if (!hasDataCategories)
                {
                    previousPeriodResults = await searchInsightsRepo.GetCountAsync(searchIndexId, previousDateFrom, previousDateTo);
                }
                else
                {
                    previousPeriodResults = await searchInsightsRepo.GetByCategoriesAsync(searchIndexId,
                                                                                          previousDateFrom,
                                                                                          previousDateTo,
                                                                                          string.Join(",", dataCategories));
                }

                var totalPreviousCount = previousPeriodResults.Sum(x => x.Count);
                var percentageChange = percentageChangeProvider.Get(tile.Count, totalPreviousCount);

                tile.PreviousPeriod = GetPreviousPeriodMessage(daysDifference);
                tile.PreviousPeriodPercentageChange = percentageChange;
                tile.IsIncreaseFromPreviousPeriod = percentageChange > 0;
            }

            return tile;
        }

        public async Task<SearchInsightSummary> GetSummaryAsync(Guid searchIndexId)
        {
            var summary = new SearchInsightSummary();

            var today = DateTime.Today;
            var dateFrom = today.AddDays(-6);

            var tasks = new List<Task>();

            //Tile 1: Searches Today
            var searchesToday = GetTileByNameAsync(searchIndexId, today, today, SearchInsightReportNames.Searches, true);
            tasks.Add(searchesToday);

            //Tile 2: Searches This Week
            var searchesThisWeek = GetTileByNameAsync(searchIndexId, dateFrom, today, SearchInsightReportNames.Searches, true);
            tasks.Add(searchesThisWeek);

            //Tile 3: Zero Results Today
            var zeroResultSearchesToday = GetTileByNameAsync(searchIndexId, today, today, SearchInsightReportNames.AllZeroResultSearches, true);
            tasks.Add(zeroResultSearchesToday);

            //Tile 4: Zero Results This Week
            var zeroResultSearchesThisWeek = GetTileByNameAsync(searchIndexId, dateFrom, today, SearchInsightReportNames.AllZeroResultSearches, true);
            tasks.Add(zeroResultSearchesThisWeek);

            //Chart 1: Searches Per Day
            var searchesPerDay = GetChartByNameAsync(searchIndexId, dateFrom, today, SearchInsightReportNames.SearchesPerDay);
            tasks.Add(searchesPerDay);

            //Chart 2: Popular Makes
            var popularMakes = GetChartByNameAsync(searchIndexId, dateFrom, today, SearchInsightReportNames.PopularMakes);
            tasks.Add(popularMakes);

            await Task.WhenAll(tasks);

            summary.Tiles = new List<SearchInsightTile>()
            {
                searchesToday.Result,
                searchesThisWeek.Result,
                zeroResultSearchesToday.Result,
                zeroResultSearchesThisWeek.Result
            };

            var allChartInsights = new List<SearchInsightChart>()
            {
                searchesPerDay.Result,
                popularMakes.Result
            };

            summary.Charts = allChartInsights;

            return summary;
        }

        private static string[] TryGetDataCategories(string name)
        {
            SearchInsightReportNames.AllWithDataCategories.TryGetValue(name, out var dataCategories);
            return dataCategories;
        }

        private static string GetPreviousPeriodMessage(int daysDifference)
        {
            return Math.Abs(daysDifference) == 1 ? "Yesterday" : $"Previous {Math.Abs(daysDifference)} days";
        }

        private static List<SearchInsight> GroupResults(IEnumerable<SearchInsight> allResults)
        {
            var groupedResults = allResults.GroupBy(x => new { x.DataCategory, x.DataPoint }).ToList();

            var newList = new List<SearchInsight>();

            foreach (var group in groupedResults)
            {
                newList.Add(new SearchInsight()
                {
                    DataCategory = group.Key.DataCategory,
                    DataPoint = group.Key.DataPoint,
                    Count = group.Sum(x => x.Count)
                });
            }

            return newList;
        }
    }
}
