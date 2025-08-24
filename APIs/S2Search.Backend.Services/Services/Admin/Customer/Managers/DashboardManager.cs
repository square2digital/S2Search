using S2Search.Backend.Domain.Customer.Dashboard;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Services.Services.Admin.Customer.Managers
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
