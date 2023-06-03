using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Providers;

namespace Tests.Services.Providers.DateTimeCategoryProviderTests
{
    public class TestsBase
    {
        internal DateTimeCategoryProvider provider;

        [TestInitialize]
        public void TestInit()
        {
            provider = new DateTimeCategoryProvider();
        }
    }
}
