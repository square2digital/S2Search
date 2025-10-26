using System;

namespace S2Search.Backend.Domain.Interfaces.SearchInsights.Providers
{
    public interface IDateTimeCategoryProvider
    {
        /// <summary>
        /// Returns a category for the part of day e.g. Morning, Afternoon, Evening and Night
        /// </summary>
        /// <param name="timeOfDay"></param>
        string GetPartOfDay(TimeSpan timeOfDay);

        /// <summary>
        /// Returns a category for the part of the week e.g. Weekday or Weekend
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        string GetPartOfWeek(DateTime date);

        /// <summary>
        /// Returns a category for the quarter of the year e.g. Q1, Q2
        /// </summary>
        /// <param name="date"></param>
        string GetQuarterOfYear(DateTime date);
    }
}
