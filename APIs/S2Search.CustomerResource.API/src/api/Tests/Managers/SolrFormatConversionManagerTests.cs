using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Interfaces.Managers;
using System.Linq;

namespace Services.Managers.Tests
{
    [TestClass()]
    public class SolrFormatConversionManagerTests
    {
        private ISolrFormatConversionManager _systemUnderTest;

        [TestInitialize]
        public void Setup()
        {
            _systemUnderTest = new SolrFormatConversionManager();
        }

        [TestMethod()]
        public void GetSolrFormatBMWTest()
        {
            string keyWord = "BMW";
            var synonyms = new string[] { "beema", "bimma" };
            string expectedResult = "beema,bimma => BMW";

            var result = _systemUnderTest.GetSolrFormat(keyWord, synonyms);

            Assert.IsTrue(result == expectedResult);
        }

        [TestMethod()]
        public void GetSolrFormatVolkswagenTest()
        {
            string keyWord = "Volkswagen";
            var synonyms = new string[] { "vw", "vdub" };
            string expectedResult = "vw,vdub => Volkswagen";

            var result = _systemUnderTest.GetSolrFormat(keyWord, synonyms);

            Assert.IsTrue(result == expectedResult);
        }

        [TestMethod()]
        public void GetSolrFormatFailTest()
        {
            string keyWord = "Volkswagen";
            var synonyms = new string[] { "vw", "vdub" };
            string unexpectedResult = "vw   ,   vdub => Volkswagen";

            var result = _systemUnderTest.GetSolrFormat(keyWord, synonyms);

            Assert.IsTrue(result != unexpectedResult);

        }

        [TestMethod()]
        public void GetSolrFormatSeperatorTest()
        {
            string solrSeperator = "=>";
            string keyWord = "Volkswagen";
            var synonyms = new string[] { "vw", "vdub" };
            string unexpectedResult = "vw,vdub = Volkswagen";

            var result = _systemUnderTest.GetSolrFormat(keyWord, synonyms);

            Assert.IsTrue(result != unexpectedResult);
            Assert.IsTrue(result.Contains(keyWord));
            Assert.IsTrue(result.Contains(solrSeperator));
        }

        [TestMethod()]
        public void GetSynonymsVolkswagenTest()
        {
            string solrFormat = "vw,vdub => Volkswagen";
            string[] expectedSynonyms = new string[] { "vw", "vdub" };

            var result = _systemUnderTest.GetSynonyms(solrFormat).ToArray();

            CollectionAssert.AreEqual(result, expectedSynonyms);
        }

        [TestMethod()]
        public void GetSynonymsBMWTest()
        {
            string solrFormat = "beema,bimma => BMW";
            string[] expectedSynonyms = new string[] { "beema", "bimma" };

            var result = _systemUnderTest.GetSynonyms(solrFormat).ToArray();

            CollectionAssert.AreEqual(result, expectedSynonyms);
        }
    }
}