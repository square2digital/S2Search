using Domain.Customer.Dashboard;
using Services.Customer.Interfaces.Managers;
using Services.Customer.Interfaces.Providers;
using Services.Customer.Interfaces.Repositories;

namespace Services.Customer.Managers
{
    public class DashboardManager : IDashboardManager
    {
        private readonly IDashboardRepository _dashboardRepo;
        private readonly IDateTimeProvider _dateTimeProvider;
        private const int _maxNumberOfHours = 1;

        public DashboardManager(IDashboardRepository dashboardRepo, IDateTimeProvider dateTimeProvider)
        {
            _dashboardRepo = dashboardRepo;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<IEnumerable<DashboardSummaryItem>> GetSummaryItems(Guid customerId)
        {
            var endDate = _dateTimeProvider.CurrentDateTime();
            var startDate = endDate.AddHours(-_maxNumberOfHours);

            var results = await _dashboardRepo.GetSummaryItems(customerId, startDate, endDate);

            return results;
        }
    }
}
