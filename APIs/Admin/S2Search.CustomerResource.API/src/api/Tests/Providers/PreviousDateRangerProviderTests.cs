using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Providers;
using System;
using System.Collections.Generic;

namespace Tests.Providers
{
    [TestClass]
    public class PreviousDateRangerProviderTests
    {
        public PreviousDateRangeProvider previousDateRange;

        [TestInitialize]
        public void Setup()
        {
            previousDateRange = new PreviousDateRangeProvider();
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDatesToTest), DynamicDataSourceType.Method)]
        public void ReturnsExpectedDateRangeAndDaysDifference(string nameOfTest,
                                                              DateTime dateFrom,
                                                              DateTime dateTo,
                                                              DateTime expectedPreviousDateFrom,
                                                              DateTime expectedPreviousDateTo,
                                                              int expectedDaysDifference)
        {
            var (previousDateFrom, previousDateTo, daysDifference) = previousDateRange.Get(dateFrom, dateTo);

            previousDateFrom.Should().BeSameDateAs(expectedPreviousDateFrom);
            previousDateTo.Should().BeSameDateAs(expectedPreviousDateTo);
            daysDifference.Should().Be(expectedDaysDifference);
        }

        private static IEnumerable<object[]> GetDatesToTest()
        {
            yield return new object[]
            {
                "1 days data",
                new DateTime(2021,9,13),
                new DateTime(2021,9,13),
                new DateTime(2021,9,12),
                new DateTime(2021,9,12),
                -1
            };

            yield return new object[]
            {
                "2 days data",
                new DateTime(2021,9,13),
                new DateTime(2021,9,14),
                new DateTime(2021,9,11),
                new DateTime(2021,9,12),
                -2
            };

            yield return new object[]
            {
                "3 day data",
                new DateTime(2021,9,13),
                new DateTime(2021,9,15),
                new DateTime(2021,9,10),
                new DateTime(2021,9,12),
                -3
            };

            yield return new object[]
            {
                "4 day data",
                new DateTime(2021,9,13),
                new DateTime(2021,9,16),
                new DateTime(2021,9,9),
                new DateTime(2021,9,12),
                -4
            };

            yield return new object[]
            {
                "7 days data",
                new DateTime(2021,9,13),
                new DateTime(2021,9,19),
                new DateTime(2021,9,6),
                new DateTime(2021,9,12),
                -7
            };

            yield return new object[]
            {
                "7 days data across a leap day",
                new DateTime(2020,2,24),
                new DateTime(2020,3,1),
                new DateTime(2020,2,17),
                new DateTime(2020,2,23),
                -7
            };

            yield return new object[]
            {
                "7 days data start on a leap day",
                new DateTime(2020,2,29),
                new DateTime(2020,3,6),
                new DateTime(2020,2,22),
                new DateTime(2020,2,28),
                -7
            };

            yield return new object[]
            {
                "7 days data end on a leap day",
                new DateTime(2020,2,23),
                new DateTime(2020,2,29),
                new DateTime(2020,2,16),
                new DateTime(2020,2,22),
                -7
            };

            yield return new object[]
            {
                "14 days data",
                new DateTime(2021,9,13),
                new DateTime(2021,9,26),
                new DateTime(2021,8,30),
                new DateTime(2021,9,12),
                -14
            };

            yield return new object[]
            {
                "30 days data",
                new DateTime(2021,9,13),
                new DateTime(2021,10,12),
                new DateTime(2021,8,14),
                new DateTime(2021,9,12),
                -30
            };

            yield return new object[]
            {
                "60 days data",
                new DateTime(2021,8,30),
                new DateTime(2021,10,28),
                new DateTime(2021,7,1),
                new DateTime(2021,8,29),
                -60
            };


        }
    }
}
