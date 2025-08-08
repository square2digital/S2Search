using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace S2Search.Backend.Tests.Search
{
    [TestClass]
    public class SearchFilterFormatterTests
    {
        [TestMethod]
        public void Test_filterformatter_make()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make eq 'bmw'");
            filterList.Add("|make eq 'bentley'");

            // act
            SearchFilterFormatterElastic filterFormatter = new SearchFilterFormatterElastic();
            var result = filterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make eq 'bmw' or make eq 'bentley')", $"actual {result}");
        }

        [TestMethod]
        public void Test_filterformatter_make_and_singlecolour()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make eq 'Lexus'");
            filterList.Add("|make eq 'audi'");
            filterList.Add("|color eq 'black'");

            // act
            SearchFilterFormatterElastic filterFormatter = new SearchFilterFormatterElastic();
            var result = filterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make eq 'Lexus' or make eq 'audi') and color eq 'black'", $"actual {result}");
        }

        [TestMethod]
        public void Test_filterformatter_make_and_multicolour()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make eq 'Lexus'");
            filterList.Add("|make eq 'audi'");
            filterList.Add("|color eq 'black'");
            filterList.Add("|color eq 'yellow'");

            // act
            SearchFilterFormatterElastic filterFormatter = new SearchFilterFormatterElastic();
            var result = filterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make eq 'Lexus' or make eq 'audi') and (color eq 'black' or color eq 'yellow')", $"actual {result}");
        }

        [TestMethod]
        public void Test_filterformatter_price()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("monthlyPrice gt 10000");
            filterList.Add("|monthlyPrice lt 30000");

            // act
            SearchFilterFormatterElastic filterFormatter = new SearchFilterFormatterElastic();
            var result = filterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(monthlyPrice gt 10000 or monthlyPrice lt 30000)", $"actual {result}");
        }

        [TestMethod]
        public void Test_filterformatter_price_ge_le()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("monthlyPrice ge 100");
            filterList.Add("|monthlyPrice le 5000");

            // act
            SearchFilterFormatterElastic filterFormatter = new SearchFilterFormatterElastic();
            var result = filterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(monthlyPrice ge 100 or monthlyPrice le 5000)", $"actual {result}");
        }

        [TestMethod]
        public void Test_filterformatter_fullset()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make eq 'Lexus'");
            filterList.Add("|make eq 'audi'");
            filterList.Add("|model eq 'continental'");
            filterList.Add("|transmission eq 'manual'");
            filterList.Add("|transmission eq 'automatic'");
            filterList.Add("|fuelType eq 'petrol'");
            filterList.Add("|year gt '1990'");
            filterList.Add("|color eq 'yellow'");
            filterList.Add("|bodyStyle eq 'SUV'");

            // act
            SearchFilterFormatterElastic filterFormatter = new SearchFilterFormatterElastic();
            var result = filterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make eq 'Lexus' or make eq 'audi') and model eq 'continental' and (transmission eq 'manual' or transmission eq 'automatic') and fuelType eq 'petrol' and year gt '1990' and color eq 'yellow' and bodyStyle eq 'SUV'", $"actual {result}");
        }

        [TestMethod]
        public void Test_two_makes_one_model()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make eq 'Nissan'");
            filterList.Add("model eq 'Pixo'");
            filterList.Add("make eq 'Land Rover'");

            // act
            SearchFilterFormatterElastic filterFormatter = new SearchFilterFormatterElastic();
            var result = filterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make eq 'Nissan' or make eq 'Land Rover') and model eq 'Pixo'", $"actual {result}");
        }

        [TestMethod]
        public void Test_two_makes_two_models()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make eq 'Honda'");
            filterList.Add("make eq 'Kia'");
            filterList.Add("model eq 'Sorento'");
            filterList.Add("model eq 'jazz'");

            // act
            SearchFilterFormatterElastic filterFormatter = new SearchFilterFormatterElastic();
            var result = filterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make eq 'Honda' or make eq 'Kia') and (model eq 'Sorento' or model eq 'jazz')", $"actual {result}");
        }

        [TestMethod]
        public void Test_four_makes_one_model()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make eq 'Honda'");
            filterList.Add("make eq 'Porsche'");
            filterList.Add("make eq 'BMW'");
            filterList.Add("make eq 'Volvo'");
            filterList.Add("model eq 'M3'");

            // act
            SearchFilterFormatterElastic filterFormatter = new SearchFilterFormatterElastic();
            var result = filterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "(make eq 'Honda' or make eq 'Porsche' or make eq 'BMW' or make eq 'Volvo') and model eq 'M3'", $"actual {result}");
        }

        [TestMethod]
        public void Test_bmw_green()
        {
            // arrange
            var filterList = new List<string>();
            filterList.Add("make eq 'bmw'");
            filterList.Add("color eq 'green'");

            // act
            SearchFilterFormatterElastic filterFormatter = new SearchFilterFormatterElastic();
            var result = filterFormatter.Format(filterList);

            // asset
            Assert.AreEqual(result, "make eq 'bmw' and color eq 'green'", $"actual {result}");
        }
    }
}
