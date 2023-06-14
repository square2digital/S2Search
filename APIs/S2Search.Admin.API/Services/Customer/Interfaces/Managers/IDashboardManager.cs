using Domain.Dashboard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Managers
{
    public interface IDashboardManager
    {
        Task<IEnumerable<DashboardSummaryItem>> GetSummaryItems(Guid customerId);
    }
}
