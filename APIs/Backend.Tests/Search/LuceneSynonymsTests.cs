using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Helper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using S2SearchAPI.Client;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;

namespace S2Search.Backend.Tests.Search
{
    [TestClass]
    public class LuceneSynonymsTests : TestBase
    {
        private Mock<ISearchIndexQueryCredentialsProvider> _queryCredentialsMock = new Mock<ISearchIndexQueryCredentialsProvider>();
        private Mock<IHttpContextAccessor> _httpContextAccessor = new Mock<IHttpContextAccessor>();
        private Mock<ISynonymsHelper> _synonymsHelperMock = new Mock<ISynonymsHelper>();
        private Mock<IAzureFacetService> _azureFacetServiceMock = new Mock<IAzureFacetService>();
        private Mock<ISynonymsService> _synomynsService = new Mock<ISynonymsService>();

        private LuceneSyntaxHelper NewLuceneHelper() => new LuceneSyntaxHelper(GetLoggerMock.Object,
                                                                               GetAppsettigsMock.Object,
                                                                               _synonymsHelperMock.Object,                                                                               
                                                                               _azureFacetServiceMock.Object,
                                                                               _queryCredentialsMock.Object,
                                                                               _synomynsService.Object);

        [TestInitialize]
        public void TestInitialize()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Host = HostString.FromUriComponent("localhost");

            _httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            _synonymsHelperMock.Setup(x => x.GetSynonymsDictionary(It.IsAny<List<string>>())).Returns(GetSynonymsDictionary);
            _azureFacetServiceMock.Setup(x => x.GetOrSetDefaultFacets(It.IsAny<string>(), It.IsAny<SearchIndexQueryCredentials>())).Returns(GetFacetHelperMock.Object.GetDefaultFacetsFromLocal());
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_Beema()
        {
            // arrange
            var searchStr = "Beema";
            var luceneHelper = NewLuceneHelper();

            // act
            var result = luceneHelper.GenerateLuceneSearchString(searchStr, _faceCacheKey);

            // asset
            Assert.IsTrue(result == "Beema", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_Beema_M3_Blue()
        {
            // arrange
            var searchStr = "Beema M3 Blue";
            var luceneHelper = NewLuceneHelper();

            // act
            var result = luceneHelper.GenerateLuceneSearchString(searchStr, _faceCacheKey);

            // asset
            Assert.IsTrue(result == "Beema AND M3 AND Blue", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_vw_golf_Blue()
        {
            // arrange
            var searchStr = "vw golf Blue";
            var luceneHelper = NewLuceneHelper();

            // act
            var result = luceneHelper.GenerateLuceneSearchString(searchStr, _faceCacheKey);

            // asset
            Assert.IsTrue(result == "vw AND golf AND Blue", $"actual {result}");
        }
    }
}
