using Services.Helpers;

namespace Tests
{
    [TestClass]
    public class GenericResponseHelperTests : TestBase
    {
        [TestMethod]
        public void Test_BuildGenericResponse_Data()
        {
            // arrange
            var json = base.GetGenericTestData;
            var indexSchema = base.GetGenericIndexData;

            // act
            var result = GenericResponseHelper.BuildGenericResponse(json, indexSchema);

            // asset
            Assert.IsTrue(result != null, $"actual {result}");
            Assert.IsTrue(result.Count > 0, $"actual {result}");
        }
    }
}