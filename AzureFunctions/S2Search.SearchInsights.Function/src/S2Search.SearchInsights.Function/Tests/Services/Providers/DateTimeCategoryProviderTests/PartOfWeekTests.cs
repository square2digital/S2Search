using Domain.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests.Services.Providers.DateTimeCategoryProviderTests
{
    [TestClass]
    public class PartOfWeekTests : TestsBase
    {
        [DataTestMethod]
        [DynamicData(nameof(GetDatesForPartOfWeekTests), DynamicDataSourceType.Method)]
        public void ReturnsExpectedPartOfWeek_WhenDateTimeProvided(DateTime date, string expectedResult)
        {
            var result = provider.GetPartOfWeek(date);
            Assert.IsTrue(result == expectedResult, $"Actual: {result} | Expected {expectedResult}");
        }

        private static IEnumerable<object[]> GetDatesForPartOfWeekTests()
        {
            yield return new object[]
            {
                new DateTime(2021,11,22, 0, 0, 0),
                TimeOfWeek.Weekday
            };

            yield return new object[]
            {
                new DateTime(2021,11,23, 0, 0, 0),
                TimeOfWeek.Weekday
            };

            yield return new object[]
            {
                new DateTime(2021,11,24, 0, 0, 0),
                TimeOfWeek.Weekday
            };

            yield return new object[]
            {
                new DateTime(2021,11,25, 0, 0, 0),
                TimeOfWeek.Weekday
            };

            yield return new object[]
            {
                new DateTime(2021,11,26, 0, 0, 0),
                TimeOfWeek.Weekday
            };

            yield return new object[]
            {
                new DateTime(2021,11,27, 0, 0, 0),
                TimeOfWeek.Weekend
            };

            yield return new object[]
            {
                new DateTime(2021,11,28, 0, 0, 0),
                TimeOfWeek.Weekend
            };
        }
    }
}
