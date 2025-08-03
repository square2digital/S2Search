using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Services.Managers.Tests
{
    [TestClass()]
    public class ValidateQueryKeyNameManagerTests
    {
        private QueryKeyNameValidationManager _systemUnderTest;

        [TestInitialize]
        public void Setup()
        {
            _systemUnderTest = new QueryKeyNameValidationManager();
        }

        [TestMethod()]
        public void IsValidShouldPassTest()
        {
            var acceptableNames = new List<string>()
            {
                "ThisNameIsAcceptable",
                "This_Name_Is_Acceptable",
                "This-Name-Is-Acceptable",
                "-ThisNameIsAcceptable-",
                "_ThisNameIsAcceptable_",
                "This-Name_Is-Acceptable"
            };

            foreach (string name in acceptableNames)
            {
                bool valid = _systemUnderTest.IsValid(name, out string errorMessage);

                Assert.IsTrue(valid);
            }
        }

        [TestMethod()]
        public void IsValidShouldFailTest()
        {
            var unacceptableNames = new List<string>()
            {
                "ThisNameIsUnacceptableBecauseItIsTooManyCharacters",
                "ThisNameIsUnacceptable BecauseSpace",
                "#This-Name-Is-Unacceptable#",
                @"\ThisNameIsUnacceptable/",
                "£$%^&ThisNameIsUnacceptable_",
                "><<>?This-Name_Is-Unacceptable"
            };

            foreach (string name in unacceptableNames)
            {
                bool valid = _systemUnderTest.IsValid(name, out string errorMessage);

                Assert.IsFalse(valid);
            }
        }
    }
}