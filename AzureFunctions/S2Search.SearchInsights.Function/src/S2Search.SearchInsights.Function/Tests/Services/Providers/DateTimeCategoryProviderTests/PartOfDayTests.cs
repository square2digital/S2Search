using Domain.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests.Services.Providers.DateTimeCategoryProviderTests
{
    [TestClass]
    public class PartOfDayTests : TestsBase
    {
        [DataTestMethod]
        [DynamicData(nameof(GetDatesForPartOfDayTests), DynamicDataSourceType.Method)]
        public void ReturnsExpectedPartOfDay_WhenDateTimeProvided(DateTime date, string expectedResult)
        {
            var result = provider.GetPartOfDay(date.TimeOfDay);
            Assert.IsTrue(result == expectedResult, $"Actual: {result} | Expected {expectedResult}");
        }

        private static IEnumerable<object[]> GetDatesForPartOfDayTests()
        {
            yield return new object[]
            {
                new DateTime(2021,9,12, 6, 0, 0),
                TimeOfDay.Morning
            };

            yield return new object[]
            {
                new DateTime(2021,9,12, 7, 0, 0),
                TimeOfDay.Morning
            };

            yield return new object[]
            {
                new DateTime(2021,9,12, 11, 0, 0),
                TimeOfDay.Morning
            };

            yield return new object[]
            {
                new DateTime(2021,9,12, 12, 0, 0),
                TimeOfDay.Afternoon
            };

            yield return new object[]
            {
                new DateTime(2021,9,12, 17, 0, 0),
                TimeOfDay.Afternoon
            };

            yield return new object[]
            {
                new DateTime(2021,9,12, 18, 0, 0),
                TimeOfDay.Evening
            };

            yield return new object[]
            {
                new DateTime(2021,9,12, 21, 0, 0),
                TimeOfDay.Evening
            };

            yield return new object[]
            {
                new DateTime(2021,9,12, 22, 0, 0),
                TimeOfDay.Night
            };

            yield return new object[]
            {
                new DateTime(2021,9,12, 5, 0, 0),
                TimeOfDay.Night
            };
        }
    }
}
