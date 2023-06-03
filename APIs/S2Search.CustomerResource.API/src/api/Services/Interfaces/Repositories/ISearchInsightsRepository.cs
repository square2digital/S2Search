using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface ISearchInsightsRepository
    {
        Task<IEnumerable<SearchInsight>> GetByCategoriesAsync(Guid searchIndexId, DateTime dateFrom, DateTime dateTo, string dataCategories);
        Task<IEnumerable<SearchInsight>> GetCountAsync(Guid searchIndexId, DateTime dateFrom, DateTime dateTo);
    }
}
