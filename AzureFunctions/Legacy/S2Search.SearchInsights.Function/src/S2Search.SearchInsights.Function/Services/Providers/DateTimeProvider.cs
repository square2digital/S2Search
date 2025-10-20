using Services.Interfaces.Providers;
using System;

namespace Services.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
