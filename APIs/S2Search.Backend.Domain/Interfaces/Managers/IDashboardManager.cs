using S2Search.Backend.Domain.Customer.Dashboard;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

public interface IDashboardManager
{
    Task<IEnumerable<DashboardSummaryItem>> GetSummaryItems(Guid customerId);
}
