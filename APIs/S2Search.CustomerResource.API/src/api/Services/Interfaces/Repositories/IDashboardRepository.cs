using Domain.Dashboard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<DashboardSummaryItem>> GetSummaryItems(Guid customerId, DateTime startDate, DateTime endDate);
    }
}
