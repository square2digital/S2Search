namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories
{
    public interface ISearchInsightsReportRepository
    {
        bool Exists(string reportName);
    }
}
