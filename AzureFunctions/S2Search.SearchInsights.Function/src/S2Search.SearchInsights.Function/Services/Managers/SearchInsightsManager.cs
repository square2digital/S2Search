using Domain.Models;
using Services.Interfaces.Managers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Managers
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
