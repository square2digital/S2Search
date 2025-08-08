using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Services.Services.Admin.Customer.Repositories
{
    public class SearchInsightsReportRepository : ISearchInsightsReportRepository
    {
        public bool Exists(string reportName)
        {
            return SearchInsightReportNames.Reports.Contains(reportName);
        }
    }
}
