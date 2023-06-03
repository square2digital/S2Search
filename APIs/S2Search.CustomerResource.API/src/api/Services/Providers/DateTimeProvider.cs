using Services.Interfaces.Providers;
using System;

namespace Services.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime CurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
