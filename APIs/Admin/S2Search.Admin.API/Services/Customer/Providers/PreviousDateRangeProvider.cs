using Services.Customer.Interfaces.Providers;

namespace Services.Customer.Providers
{
    public class PreviousDateRangeProvider : IPreviousDateRangeProvider
    {
        public (DateTime previousDateFrom, DateTime previousDateTo, int daysDifference) Get(DateTime currentDateFrom, DateTime currentDateTo)
        {
            var totalDaysDifference = (currentDateFrom.Date - currentDateTo.Date).TotalDays;

            //increase the difference by 1 to account for the current day as a chunk of data
            //e.g. 13/09/2021 to 14/09/2021 counts as 2 days worth of data but the TotalDays = 1
            totalDaysDifference--; 

            var previousDateFrom = currentDateFrom.AddDays(totalDaysDifference);
            var previousDateTo = currentDateTo.AddDays(totalDaysDifference);

            return (previousDateFrom, previousDateTo, (int)totalDaysDifference);
        }
    }
}
