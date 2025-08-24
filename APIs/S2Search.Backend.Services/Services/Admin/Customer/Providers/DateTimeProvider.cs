using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime CurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
