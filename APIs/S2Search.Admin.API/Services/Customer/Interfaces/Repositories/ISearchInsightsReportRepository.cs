namespace Services.Customer.Interfaces.Repositories
{
    public interface ISearchInsightsReportRepository
    {
        bool Exists(string reportName);
    }
}
