using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Managers;

namespace Tests.Services.Managers.SearchFacetFormatManagerTests
{
    public class TestsBase
    {
        internal SearchFacetsFormatManager formatManager;

        [TestInitialize]
        public void TestInitialize()
        {
            formatManager = new SearchFacetsFormatManager();
        }
    }
}
