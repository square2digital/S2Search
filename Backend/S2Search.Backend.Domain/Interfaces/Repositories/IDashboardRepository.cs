using S2Search.Backend.Domain.Customer.Dashboard;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

public interface IDashboardRepository
{
    Task<IEnumerable<DashboardSummaryItem>> GetSummaryItems(Guid customerId, DateTime startDate, DateTime endDate);
}
