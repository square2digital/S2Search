using System;

namespace Services.Helpers
{
    public static class CacheHelper
    {
        private static readonly int _fallbackValue = 3600;

        /// <summary>
        /// Will return a timespan in seconds from the paramater - if the paramater is 0 or null it will fall back to the default of 1 hour (3600 seconds)
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan GetExpiry(int? seconds)
        {
            if(!seconds.HasValue || seconds == 0)
            {
                return TimeSpan.FromSeconds(_fallbackValue);
            }

            return TimeSpan.FromSeconds(seconds.Value);
        }
    }
}
