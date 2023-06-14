namespace Services.Interfaces.Repositories
{
    public interface ISearchInsightsReportRepository
    {
        bool Exists(string reportName);
    }
}
