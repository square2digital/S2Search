﻿using Domain.Constants;
using Domain.Models;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class SearchInsightsRepository : ISearchInsightsRepository
    {
        private readonly IDbContextProvider _dbContext;

        public SearchInsightsRepository(IDbContextProvider dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<SearchInsight>> GetByCategoriesAsync(Guid searchIndexId,
                                                                           DateTime dateFrom,
                                                                           DateTime dateTo,
                                                                           string dataCategories)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "DateFrom", dateFrom },
                { "DateTo", dateTo },
                { "DataCategories", dataCategories }
            };

            var result = await _dbContext.QueryAsync<SearchInsight>(ConnectionStrings.CustomerResourceStore,
                                                                    StoredProcedures.GetSearchInsightsByDataCategories,
                                                                    parameters);

            return result;
        }

        public async Task<IEnumerable<SearchInsight>> GetCountAsync(Guid searchIndexId, DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchIndexId },
                { "DateFrom", dateFrom },
                { "DateTo", dateTo },
            };

            var result = await _dbContext.QueryAsync<SearchInsight>(ConnectionStrings.CustomerResourceStore,
                                                                    StoredProcedures.GetSearchInsightsSearchCountByDateRange,
                                                                    parameters);

            return result;
        }
    }
}
