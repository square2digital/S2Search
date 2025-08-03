using System;

namespace Services.Customer.Interfaces.Providers
{
    public interface IPreviousDateRangeProvider
    {
        /// <summary>
        /// Returns the previous date range based on the current date range passed in.
        /// For example the below would count as 7 whole days worth of data:
        /// <br/>
        /// <br/>
        /// <paramref name="currentDateFrom"/> = 01/11/2021
        /// <br/>
        /// <paramref name="currentDateTo"/>   = 07/11/2021
        /// <br/>
        /// <br/>
        /// The previous date range would be:
        /// <br/>
        /// <br/>
        /// <b>previousDateFrom</b> = 25/10/2021
        /// <br/>
        /// <b>previousDateTo</b>   = 31/10/2021
        /// <br/>
        /// <b>daysDifference</b>   = -7
        /// </summary>
        /// <param name="currentDateFrom"></param>
        /// <param name="currentDateTo"></param>
        /// <returns></returns>
        (DateTime previousDateFrom, DateTime previousDateTo, int daysDifference) Get(DateTime currentDateFrom, DateTime currentDateTo);
    }
}
