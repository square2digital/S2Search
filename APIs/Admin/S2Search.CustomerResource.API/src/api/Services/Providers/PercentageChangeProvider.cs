using Services.Interfaces.Providers;
using System;

namespace Services.Providers
{
    public class PercentageChangeProvider : IPercentageChangeProvider
    {
        public double Get(double current, double previous)
        {
            if (previous == 0)
                return 0;

            if (current == 0)
                return -100;

            var percentageChange = ((current - previous) / previous) * 100;
            return Math.Round(percentageChange, 1);
        }
    }
}
