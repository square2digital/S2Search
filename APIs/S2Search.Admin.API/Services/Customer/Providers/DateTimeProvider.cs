using Services.Customer.Interfaces.Providers;

namespace Services.Customer.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime CurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
