using Domain.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Builders;

namespace Tests.Services.Managers.SearchFacetFormatManagerTests
{
    [TestClass]
    public class EqualityFacetTests : TestsBase
    {
        [TestMethod]
        public void ReturnsExpectedList_WhenASingleFacetIsProvidedAndValueIsAString_UsingVehicleBodyStyleExample()
        {
            //arrange
            var unformattedFacets = "bodyStyle eq 'Pick Up'";

            var expectedFacet = new SearchFacet() { Category = "Bodystyle", Value = "Pick Up" };
            var expectedList = new SearchFacetListBuilder().AddSearchFacets(expectedFacet).Build();

            //act
            var result = formatManager.GetSearchFacets(unformattedFacets);

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().BeEquivalentTo(expectedList);
        }

        [TestMethod]
        public void ReturnsExpectedList_WhenASingleFacetIsProvidedAndValueIsAString_UsingVehicleDoorsExample()
        {
            //arrange
            var unformattedFacets = "doors eq 3";

            var expectedFacet = new SearchFacet() { Category = "Doors", Value = "3" };
            var expectedList = new SearchFacetListBuilder().AddSearchFacets(expectedFacet).Build();

            //act
            var result = formatManager.GetSearchFacets(unformattedFacets);

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().BeEquivalentTo(expectedList);
        }

        [TestMethod]
        public void ReturnsExpectedList_WhenMultipleFacetsAreProvidedAndValueIsAString_UsingVehicleMakesExample()
        {
            //arrange
            var unformattedFacets = "make eq 'Subaru',make eq 'Suzuki',make eq 'Toyota',make eq 'Volvo'";

            var expectedFacets = new SearchFacet[] 
            {
                new SearchFacet() { Category = "Make", Value = "Subaru" },
                new SearchFacet() { Category = "Make", Value = "Suzuki" },
                new SearchFacet() { Category = "Make", Value = "Toyota" },
                new SearchFacet() { Category = "Make", Value = "Volvo" }
            };
            var expectedList = new SearchFacetListBuilder().AddSearchFacets(expectedFacets).Build();

            //act
            var result = formatManager.GetSearchFacets(unformattedFacets);

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(4);
            result.Should().BeEquivalentTo(expectedList);
        }

        [TestMethod]
        public void ReturnsExpectedList_WhenMultipleFacetsAreProvidedAndValueIsNumeric_UsingVehicleDoorsExample()
        {
            //arrange
            var unformattedFacets = "doors eq 2,doors eq 3,doors eq 4,doors eq 5";

            var expectedFacets = new SearchFacet[]
            {
                new SearchFacet() { Category = "Doors", Value = "2" },
                new SearchFacet() { Category = "Doors", Value = "3" },
                new SearchFacet() { Category = "Doors", Value = "4" },
                new SearchFacet() { Category = "Doors", Value = "5" }
            };
            var expectedList = new SearchFacetListBuilder().AddSearchFacets(expectedFacets).Build();

            //act
            var result = formatManager.GetSearchFacets(unformattedFacets);

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(4);
            result.Should().BeEquivalentTo(expectedList);
        }

        [TestMethod]
        public void ReturnsExpectedList_WhenMultipleFacetsAreProvidedAndValueIsAString_UsingRandomExample()
        {
            //arrange
            var unformattedFacets = "someFacetName eq 'Some Product Value 1',someFacetName eq 'Some Product Value 2',someFacetName eq 'Some Product Value 3'";

            var expectedFacets = new SearchFacet[]
            {
                new SearchFacet() { Category = "Somefacetname", Value = "Some Product Value 1" },
                new SearchFacet() { Category = "Somefacetname", Value = "Some Product Value 2" },
                new SearchFacet() { Category = "Somefacetname", Value = "Some Product Value 3" }
            };
            var expectedList = new SearchFacetListBuilder().AddSearchFacets(expectedFacets).Build();

            //act
            var result = formatManager.GetSearchFacets(unformattedFacets);

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(expectedList);
        }

        [TestMethod]
        public void ReturnsExpectedList_WhenMultipleFacetsAreProvidedAndValueIsNumeric_UsingRandomExample()
        {
            //arrange
            var unformattedFacets = "someFacetName eq 1,someFacetName eq 2,someFacetName eq 3";

            var expectedFacets = new SearchFacet[]
            {
                new SearchFacet() { Category = "Somefacetname", Value = "1" },
                new SearchFacet() { Category = "Somefacetname", Value = "2" },
                new SearchFacet() { Category = "Somefacetname", Value = "3" }
            };
            var expectedList = new SearchFacetListBuilder().AddSearchFacets(expectedFacets).Build();

            //act
            var result = formatManager.GetSearchFacets(unformattedFacets);

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(expectedList);
        }
    }
}
