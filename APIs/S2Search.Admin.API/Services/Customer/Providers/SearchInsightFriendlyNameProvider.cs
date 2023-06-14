using Domain.Customer.Constants;
using Services.Customer.Interfaces.Providers;

namespace Services.Customer.Providers
{
    public class SearchInsightFriendlyNameProvider : ISearchInsightFriendlyNameProvider
    {
        public string Get(string reportName, DateTime dateFrom, DateTime dateTo)
        {
            var friendlyName = reportName switch
            {
                SearchInsightReportNames.PopularMakes => "Popular Makes",
                SearchInsightReportNames.SearchesPerDay => "Searches Per Day",
                SearchInsightReportNames.Searches => "Searches ##PERIOD##",
                SearchInsightReportNames.AllZeroResultSearches => "Zero Results ##PERIOD##",
                _ => throw new NotImplementedException($"{nameof(reportName)} with value '{reportName}' has not been mapped")
            };

            var isToday = DateTime.Today == dateFrom && DateTime.Today == dateTo;
            var currentDateDaysDifference = (dateFrom - dateTo).TotalDays;
            currentDateDaysDifference--;

            var periodReplacement = isToday ? "Today" : $"Last {Math.Abs(currentDateDaysDifference)} days";

            friendlyName = friendlyName.Replace("##PERIOD##", periodReplacement);

            return friendlyName;
        }
    }
}
