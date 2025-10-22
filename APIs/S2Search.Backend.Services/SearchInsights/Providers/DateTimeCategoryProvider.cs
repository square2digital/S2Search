using S2Search.Backend.Domain.AzureFunctions.SearchInsights.Constants;
using S2Search.Backend.Domain.Interfaces.SearchInsights.Providers;

namespace S2Search.Backend.Services.SearchInsights.Providers
{
    public class DateTimeCategoryProvider : IDateTimeCategoryProvider
    {
        private readonly int sixAm = 6;
        private readonly int twelvePm = 12;
        private readonly int sixPm = 18;
        private readonly int tenPm = 22;

        public string GetPartOfDay(TimeSpan timeOfDay)
        {
            var isMorning = timeOfDay.Hours >= sixAm && timeOfDay.Hours < twelvePm;

            if (isMorning)
            {
                return TimeOfDay.Morning;
            }

            var isAfternoon = timeOfDay.Hours >= twelvePm && timeOfDay.Hours < sixPm;

            if (isAfternoon)
            {
                return TimeOfDay.Afternoon;
            }

            var isEvening = timeOfDay.Hours >= sixPm && timeOfDay.Hours < tenPm;

            if (isEvening)
            {
                return TimeOfDay.Evening;
            }

            var isNight = timeOfDay.Hours >= tenPm || timeOfDay.Hours < sixAm;

            if (isNight)
            {
                return TimeOfDay.Night;
            }

            return "Unknown";
        }

        public string GetPartOfWeek(DateTime date)
        {
            if(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return TimeOfWeek.Weekend;
            }

            return TimeOfWeek.Weekday;
        }

        public string GetQuarterOfYear(DateTime date)
        {
            var quarter = (date.Month + 2) / 3;
            return $"Q{quarter}";
        }
    }
}
