using Domain.Customer.Dashboard;
namespace Services.Customer.Interfaces.Managers
{
    public interface IDashboardManager
    {
        Task<IEnumerable<DashboardSummaryItem>> GetSummaryItems(Guid customerId);
    }
}
