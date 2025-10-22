using S2Search.Backend.Domain.Interfaces.SearchInsights.Providers;

namespace S2Search.Backend.Services.Functions.SearchInsights.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
