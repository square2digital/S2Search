using Domain.SearchResources.Synonyms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Interfaces.Managers;

namespace Services.Managers.Tests
{
    [TestClass()]
    public class SynonymValidationManagerTests
    {
        private ISynonymValidationManager _systemUnderTest;

        [TestInitialize]
        public void Setup()
        {
            _systemUnderTest = new SynonymValidationManager();
        }

        [TestMethod()]
        public void IsValid_PassTest()
        {
            var synonymRequest = new SynonymRequest()
            {
                KeyWord = "BMW",
                Synonyms = new string[]
                {
                    "beema",
                    "bimma"
                },
            };
            var result = _systemUnderTest.IsValid(synonymRequest, out string errors);

            Assert.IsTrue(result);
            Assert.IsTrue(string.IsNullOrEmpty(errors));
        }

        [TestMethod()]
        public void IsValid_InvalidSpacingTest()
        {
            var synonymRequest = new SynonymRequest()
            {
                KeyWord = "BMW",
                Synonyms = new string[]
                {
                    "beema",
                    "bimma",
                    "zim  zimma"
                },
            };
            var result = _systemUnderTest.IsValid(synonymRequest, out string errors);

            Assert.IsFalse(result);
            Assert.IsFalse(string.IsNullOrEmpty(errors));
        }

        [TestMethod()]
        public void IsValid_ValidCharactersTest()
        {
            var synonymRequest = new SynonymRequest()
            {
                KeyWord = "BMW",
                Synonyms = new string[]
                {
                    "beema",
                    "bimma",
                    "zim zimma",
                    "another test synonym"
                },
            };
            var result = _systemUnderTest.IsValid(synonymRequest, out string errors);

            Assert.IsTrue(result);
            Assert.IsTrue(string.IsNullOrEmpty(errors));
        }

        [TestMethod()]
        public void IsValid_InvalidCharactersTest()
        {
            var synonymRequest = new SynonymRequest()
            {
                KeyWord = "BMW",
                Synonyms = new string[]
                {
                    "beema",
                    "bimma",
                    "zim-zimma",
                    "zim_zimma",
                    "zim#zimma",
                    "zim/zimma",
                    @"zim\zimma",
                },
            };
            var result = _systemUnderTest.IsValid(synonymRequest, out string errors);

            Assert.IsFalse(result);
            Assert.IsFalse(string.IsNullOrEmpty(errors));
        }
    }
}