using Services.Helpers;

namespace Tests
{
    [TestClass]
    public class GenericConversionServiceTests : TestBase
    {
        [TestMethod]
        public void Test_BuildGenericResponse_Data()
        {
            // arrange
            var json = base.GetGenericIndexData;
            var index = "s2-demo-generic";

            // act
            var result = IndexFacetHelper.BuildGenericFacet(json, index);

            // asset
            Assert.IsTrue(result != null, $"actual {result}");
            Assert.IsTrue(result.Count > 0, $"actual {result}");
        }

        [TestMethod]
        public void Test_GetTypeForTextType()
        {
            // arrange
            var json = base.GetGenericIndexData;

            // act
            var result = IndexHelper.GetTextTypeDictionary(json);

            // asset
            Assert.IsTrue(result["BodyStyle".ToLower()] == "keyword");
            Assert.IsTrue(result["Colour".ToLower()] == "keyword");

        }
    }
}