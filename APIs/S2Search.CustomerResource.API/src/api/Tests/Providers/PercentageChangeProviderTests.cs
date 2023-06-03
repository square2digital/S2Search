using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Interfaces.Providers;
using Services.Providers;

namespace Tests.Providers
{
    [TestClass]
    public class PercentageChangeProviderTests
    {
        private IPercentageChangeProvider percentageChangeProvider;

        [TestInitialize]
        public void Setup()
        {
            percentageChangeProvider = new PercentageChangeProvider();
        }

        [DataTestMethod]
        [DataRow(100, 50, 100)]
        [DataRow(100, 90, 11.1)]
        [DataRow(2500, 2383, 4.9)]
        [DataRow(2383, 2500, -4.7)]
        [DataRow(0, 2500, -100)]
        [DataRow(1, 0, 0)]
        public void ReturnsExpectedPercentageChange(int current, int previous, double expectedPercentageChange)
        {
            var percentageChange = percentageChangeProvider.Get(current, previous);

            percentageChange.Should().Be(expectedPercentageChange);
        }
    }
}
