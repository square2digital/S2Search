using System;

namespace Services.Customer.Interfaces.Providers
{
    public interface ISearchInsightFriendlyNameProvider
    {
        string Get(string reportName, DateTime dateFrom, DateTime dateTo);
    }
}
