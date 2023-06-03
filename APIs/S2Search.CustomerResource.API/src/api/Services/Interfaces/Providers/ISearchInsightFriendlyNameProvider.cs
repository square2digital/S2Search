using System;

namespace Services.Interfaces.Providers
{
    public interface ISearchInsightFriendlyNameProvider
    {
        string Get(string reportName, DateTime dateFrom, DateTime dateTo);
    }
}
