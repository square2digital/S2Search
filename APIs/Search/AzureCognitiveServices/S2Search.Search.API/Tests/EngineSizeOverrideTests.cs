using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Moq;
using Services.Providers;
using Services.Helper;
using Services.Helpers.FacetOverrides;

namespace Tests
{
    [TestClass]
    public class EngineSizeOverrideTests : TestBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void Test_PorscheEngineSize()
        {
            // arrange
            var testFacet = base.GetTestPorscheEngineSizeFacetItem;

            var returnFacet = new EngineSizeOverride().Override(testFacet);

            // asset
            Assert.IsTrue(returnFacet.Value == "2900");
            Assert.IsTrue(returnFacet.FacetDisplayText == "2.9 L");
        }
    }
}
