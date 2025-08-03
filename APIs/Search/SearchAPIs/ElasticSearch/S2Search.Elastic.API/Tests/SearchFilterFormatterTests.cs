using Services.Providers;

namespace Tests
{
    [TestClass]
    public class SearchFilterFormatterTests
    {
        [TestMethod]
        public void Test_filterformatter_elastic()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make:Ford");
            filterList.Add("make:BMW");
            filterList.Add("make:Nissan");

            // act
            var result = SearchFilterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make:Ford OR make:BMW OR make:Nissan)", $"actual {result}");
        }

        [TestMethod]
        public void Test_filterformatter_make()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make:bmw");
            filterList.Add("make:bentley");

            // act
            var result = SearchFilterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make:bmw OR make:bentley)", $"actual {result}");
        }

        [TestMethod]
        public void Test_filterformatter_make_and_singlecolour()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make:Lexus");
            filterList.Add("make:audi");
            filterList.Add("color:black");

            // act
            var result = SearchFilterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make:Lexus OR make:audi) AND color:black", $"actual {result}");
        }

        [TestMethod]
        public void Test_filterformatter_make_and_multicolour()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make:Lexus");
            filterList.Add("make:audi");
            filterList.Add("color:black");
            filterList.Add("color:yellow");

            // act
            var result = SearchFilterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make:Lexus OR make:audi) AND (color:black OR color:yellow)", $"actual {result}");
        }

        //[TestMethod]
        //public void Test_filterformatter_price()
        //{
        //    // arrange
        //    var filterList = new List<string>();
        //    filterList.Add("monthlyPrice gt 10000");
        //    filterList.Add("|monthlyPrice lt 30000");

        //    // act
        //    var result = SearchFilterFormatter.Format(filterList);

        //    // asset
        //    Assert.AreEqual(result, "(monthlyPrice gt 10000 or monthlyPrice lt 30000)", $"actual {result}");
        //}

        //[TestMethod]
        //public void Test_filterformatter_price_ge_le()
        //{
        //    // arrange
        //    var filterList = new List<string>();
        //    filterList.Add("monthlyPrice ge 100");
        //    filterList.Add("monthlyPrice le 5000");

        //    // act
        //    var result = SearchFilterFormatter.Format(filterList);

        //    // asset
        //    Assert.AreEqual(result, "(monthlyPrice ge 100 or monthlyPrice le 5000)", $"actual {result}");
        //}

        [TestMethod]
        public void Test_filterformatter_fullset()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make:Lexus");
            filterList.Add("make:audi");
            filterList.Add("model:continental");
            filterList.Add("transmission:manual");
            filterList.Add("transmission:automatic");
            filterList.Add("fuelType:petrol");
            filterList.Add("year:1990");
            filterList.Add("color:yellow");
            filterList.Add("bodyStyle:SUV");

            // act
            var result = SearchFilterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make:Lexus OR make:audi) AND model:continental AND (transmission:manual OR transmission:automatic) AND fuelType:petrol AND year:1990 AND color:yellow AND bodyStyle:SUV", $"actual {result}");
        }

        [TestMethod]
        public void Test_two_makes_one_model()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make:Nissan");
            filterList.Add("model:Pixo");
            filterList.Add("make:Land Rover");

            // act
            var result = SearchFilterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make:Nissan OR make:Land Rover) AND model:Pixo", $"actual {result}");
        }

        [TestMethod]
        public void Test_two_makes_two_models()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make:Honda");
            filterList.Add("make:Kia");
            filterList.Add("model:Sorento");
            filterList.Add("model:jazz");

            // act
            var result = SearchFilterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make:Honda OR make:Kia) AND (model:Sorento OR model:jazz)", $"actual {result}");
        }

        [TestMethod]
        public void Test_four_makes_one_model()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make:Honda");
            filterList.Add("make:Porsche");
            filterList.Add("make:BMW");
            filterList.Add("make:Volvo");
            filterList.Add("model:M3");

            // act
            var result = SearchFilterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make:Honda OR make:Porsche OR make:BMW OR make:Volvo) AND model:M3", $"actual {result}");
        }

        [TestMethod]
        public void Test_bmw_green()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make:(bmw)");
            filterList.Add("color:green");

            // act
            var result = SearchFilterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "make:(bmw) AND color:green", $"actual {result}");
        }
    }
}
