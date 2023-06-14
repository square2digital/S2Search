using Domain.Constants;
using Domain.Customer.Constants;
using Services.Customer.Interfaces.Repositories;

namespace Services.Customer.Repositories
{
    public class SearchInsightsReportRepository : ISearchInsightsReportRepository
    {
        public bool Exists(string reportName)
        {
            return SearchInsightReportNames.Reports.Contains(reportName);
        }
    }
}
