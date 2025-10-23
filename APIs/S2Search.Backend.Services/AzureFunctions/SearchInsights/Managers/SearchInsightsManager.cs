using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Models;
using S2Search.Backend.Domain.Interfaces.SearchInsights.Managers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Services.Functions.SearchInsights.Managers
{
    public class SearchInsightsManager : ISearchInsightsManager
    {
        private readonly ISearchInsightsRepository searchInsightsRepository;

        public SearchInsightsManager(ISearchInsightsRepository searchInsightsRepository)
        {
            this.searchInsightsRepository = searchInsightsRepository ?? throw new ArgumentNullException(nameof(searchInsightsRepository));
        }

        public async Task SaveInsightsAsync(Guid searchIndexId, IEnumerable<SearchInsightDataPoint> dataPoints, DateTime dateGenerated)
        {
            var repoTasks = new List<Task>
            {
                searchInsightsRepository.SaveSearchRequestAsync(searchIndexId, dateGenerated),
                searchInsightsRepository.SaveInsightsAsync(searchIndexId, dataPoints)
            };

            await Task.WhenAll(repoTasks);
        }
    }
}
