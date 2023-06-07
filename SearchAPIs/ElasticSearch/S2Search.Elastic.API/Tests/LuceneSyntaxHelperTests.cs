using Services.Helper;

namespace Tests
{
    [TestClass]
    public class LuceneSyntaxHelperTests : TestBase
    {
        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_MakesAndModel()
        {
            // arrange
            var searchStr = "Ford Escort";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "Ford AND Escort", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_Yellow()
        {
            // arrange
            var searchStr = "Yellow";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "Yellow", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_Yellow_Blue_Green()
        {
            // arrange
            var searchStr = "Yellow Blue Green";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "Yellow AND Blue AND Green", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_Merc_E_Class()
        {
            // arrange
            var searchStr = "e class";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "\"e class\"", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_Merc_S_Class()
        {
            // arrange
            var searchStr = "s class";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "\"s class\"", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_Merc_C_Class()
        {
            // arrange
            var searchStr = "c class";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "\"c class\"", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_Alfa_Romeo_Mito()
        {
            // arrange
            var searchStr = "Alfa Romeo Mito";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "\"Alfa Romeo\" AND Mito*", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_Ford_Fiesta_Petrol()
        {
            // arrange
            var searchStr = "Ford Fiesta Petrol";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "Ford AND Fiesta AND Petrol", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_Audi_Blue()
        {
            // arrange
            var searchStr = "Audi blue";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "Audi AND blue", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_Make_Model_3Series()
        {
            // arrange
            var searchStr = "BMW                                               3 series           ";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "BMW AND \"3 series\"", $"actual {result}");
        }

        [TestMethod]
        public void Test_Petrol_Automatic()
        {
            // arrange
            var searchStr = "Petrol                                               Automatic           ";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "Petrol AND Automatic", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_BMW_ModelWithDoors()
        {
            // arrange
            var searchStr = "BMW              3 series    4 door       ";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "BMW AND \"3 series\" AND \"4 doors\"", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_2_door_e_class()
        {
            // arrange
            var searchStr = "2 doors e class black amg petrol";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "\"2 doors\" AND \"e class\" AND black AND amg AND petrol", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_7_seats_merc()
        {
            // arrange
            var searchStr = "7 seats merc";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "\"7 seats\" AND merc*", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_Skoda_Octavia_vrs()
        {
            // arrange
            var searchStr = "Skoda Octavia vrs";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "Skoda* AND Octavia* AND vrs*", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_5_series_2011()
        {
            // arrange
            var searchStr = "5 series 2011";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "\"5 series\" AND 2011", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_bmw_text()
        {
            // arrange
            var searchStr = "320d [184] M Sport 4dr Step Auto";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "320d* AND [184]* AND M* AND Sport* AND 4dr* AND Step* AND Auto*", $"actual {result}");
        }

        [TestMethod]
        public void Test_GenerateLucenceSearchString_bmw_blue()
        {
            // arrange
            var searchStr = "bmw blue ";

            // act
            var result = LuceneSyntaxHelper.GenerateLuceneSearchString(searchStr);

            // asset
            Assert.IsTrue(result == "bmw AND blue", $"actual {result}");
        }

        [TestMethod]
        public void Test_EscapeLuceneSpecialCharacters_1()
        {
            // arrange
            var searchStr = "bmw #@ []  bl**ue ";

            // act
            var result = LuceneSyntaxHelper.EscapeLuceneSpecialCharacters(searchStr);

            // asset
            Assert.IsTrue(result == "bmw #@ \\[\\]  bl\\*\\*ue", $"actual {result}");
        }

        [TestMethod]
        public void Test_EscapeLuceneSpecialCharacters_2()
        {
            // arrange
            var searchStr = "Yel^low ?Bl-0ue Gre~#en!";

            // act
            var result = LuceneSyntaxHelper.EscapeLuceneSpecialCharacters(searchStr);

            // asset
            Assert.IsTrue(result == "Yel\\^low \\?Bl\\-0ue Gre\\~#en\\!", $"actual {result}");
        }

        [TestMethod]
        public void Test_EscapeLuceneSpecialCharacters_3()
        {
            // arrange
            var searchStr = "320d [184] M Sport 4dr Step Auto";

            // act
            var result = LuceneSyntaxHelper.EscapeLuceneSpecialCharacters(searchStr);

            // asset
            Assert.IsTrue(result == "320d \\[184\\] M Sport 4dr Step Auto", $"actual {result}");
        }

        [TestMethod]
        public void Test_EscapeLuceneSpecialCharacters_4()
        {
            // arrange
            var searchStr = "(1+1):2";

            // act
            var result = LuceneSyntaxHelper.EscapeLuceneSpecialCharacters(searchStr);

            // asset
            Assert.IsTrue(result == "\\(1\\+1\\)\\:2", $"actual {result}");
        }

        [TestMethod]
        public void Test_DoesSearchTermContainSpecialCharacters_1()
        {
            // arrange
            var searchStr = "Yel^low ?Bl-0ue Gre~#en!";

            // act
            var result = LuceneSyntaxHelper.ContainsSpecialCharacters(searchStr);

            // asset
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_DoesSearchTermContainSpecialCharacters_2()
        {
            // arrange
            var searchStr = "Yellow Blue Green";

            // act
            var result = LuceneSyntaxHelper.ContainsSpecialCharacters(searchStr);

            // asset
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_DoesSearchTermContainSpecialCharacters_3()
        {
            // arrange
            var searchStr = "Y\\\\e@llo\\ ? :::w ^Bl@ue G#re@en";

            // act
            var result = LuceneSyntaxHelper.ContainsSpecialCharacters(searchStr);

            // asset
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_Mercedes_Dash_Benz()
        {
            // arrange
            var searchStr = "Mercedes-Benz";

            // act
            var result = LuceneSyntaxHelper.EscapeLuceneSpecialCharacters(searchStr);

            // asset
            Assert.IsTrue(result == "Mercedes\\-Benz", $"actual {result}");
        }

        [TestMethod]
        public void Test_Nissan_GT_Dash_R()
        {
            // arrange
            var searchStr = "Nissan GT-R";

            // act
            var result = LuceneSyntaxHelper.EscapeLuceneSpecialCharacters(searchStr);

            // asset
            Assert.IsTrue(result == "Nissan GT\\-R", $"actual {result}");
        }
    }
}
