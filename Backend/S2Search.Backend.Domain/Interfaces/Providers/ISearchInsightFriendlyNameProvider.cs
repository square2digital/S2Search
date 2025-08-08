using System;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;

public interface ISearchInsightFriendlyNameProvider
{
    string Get(string reportName, DateTime dateFrom, DateTime dateTo);
}
