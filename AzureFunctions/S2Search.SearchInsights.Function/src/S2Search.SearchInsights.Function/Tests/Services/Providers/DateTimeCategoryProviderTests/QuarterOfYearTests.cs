using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests.Services.Providers.DateTimeCategoryProviderTests
{
    [TestClass]
    public class QuarterOfYearTests : TestsBase
    {
        private const string Q1 = nameof(Q1);
        private const string Q2 = nameof(Q2);
        private const string Q3 = nameof(Q3);
        private const string Q4 = nameof(Q4);

        [DataTestMethod]
        [DynamicData(nameof(GetDatesForQuarterOfYearTests), DynamicDataSourceType.Method)]
        public void ReturnsExpectedQuarterOfYear_WhenDateTimeProvided(DateTime date, string expectedResult)
        {
            var result = provider.GetQuarterOfYear(date);
            Assert.IsTrue(result == expectedResult, $"Actual: {result} | Expected {expectedResult}");
        }

        private static IEnumerable<object[]> GetDatesForQuarterOfYearTests()
        {
            yield return new object[]
            {
                new DateTime(2021,1,1),
                Q1
            };

            yield return new object[]
            {
                new DateTime(2021,3,31),
                Q1
            };

            yield return new object[]
            {
                new DateTime(2021,4,1),
                Q2
            };

            yield return new object[]
            {
                new DateTime(2021,6,30),
                Q2
            };

            yield return new object[]
            {
                new DateTime(2021,7,1),
                Q3
            };

            yield return new object[]
            {
                new DateTime(2021,9,30),
                Q3
            };

            yield return new object[]
            {
                new DateTime(2021,10,1),
                Q4
            };

            yield return new object[]
            {
                new DateTime(2021,12,31),
                Q4
            };
        }
    }
}
