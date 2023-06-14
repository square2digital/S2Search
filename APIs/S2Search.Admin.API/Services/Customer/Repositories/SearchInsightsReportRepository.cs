using Domain.Constants;
using Services.Interfaces.Repositories;
using System;
using System.Linq;

namespace Services.Repositories
{
    public class SearchInsightsReportRepository : ISearchInsightsReportRepository
    {
        public bool Exists(string reportName)
        {
            return SearchInsightReportNames.Reports.Contains(reportName);
        }
    }
}
