using Domain.Customer.Dashboard;

namespace Services.Customer.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<DashboardSummaryItem>> GetSummaryItems(Guid customerId, DateTime startDate, DateTime endDate);
    }
}
