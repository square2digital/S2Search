using Domain.Constants;
using Domain.Dashboard;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IDbContextProvider _dbContext;

        public DashboardRepository(IDbContextProvider dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DashboardSummaryItem>> GetSummaryItems(Guid customerId, DateTime startDate, DateTime endDate)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerId", customerId },
                { "StartDate", startDate },
                { "EndDate", endDate }
            };

            var results = await _dbContext.QueryAsync<DashboardSummaryItem>(ConnectionStrings.CustomerResourceStore,
                                                                            StoredProcedures.GetDashboardSummaryForCustomer,
                                                                            parameters);

            return results;
        }
    }
}
