using Domain.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tests.Builders;

namespace Tests.Services.Managers.SearchFacetFormatManagerTests
{
    [TestClass]
    public class RangeFacetTests : TestsBase
    {
        [TestMethod]
        public void ReturnsExpectedList_WhenASingleRangeFacetIsProvided_UsingPriceExample()
        {
            //arrange
            var unformattedFacets = "price ge 5001 and price le 6001";

            var expectedFacets = new SearchFacet[]
            {
                new SearchFacet() { Category = "Price", Value = "5001-6001" }
            };
            var expectedList = new SearchFacetListBuilder().AddSearchFacets(expectedFacets).Build();

            //act
            var result = formatManager.GetSearchFacets(unformattedFacets);

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().BeEquivalentTo(expectedList);
        }

        [TestMethod]
        public void ReturnsExpectedList_WhenMultipleRangeFacetsAreProvided_UsingPriceExample()
        {
            //arrange
            var unformattedFacets = "price ge 5001 and price le 6001,price ge 6001 and price le 7001,price ge 7001 and price le 8001";

            var expectedFacets = new SearchFacet[]
            {
                new SearchFacet() { Category = "Price", Value = "5001-6001" },
                new SearchFacet() { Category = "Price", Value = "6001-7001" },
                new SearchFacet() { Category = "Price", Value = "7001-8001" }
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
        public void ThrowsException_WhenAnInvalidRangeFacetIsProvided()
        {
            //arrange
            var unformattedFacets = "price ge 5001 and price le 6001 and price le 6001";

            var expectedExceptionMessage = "Facet 'price' is a range and does not contain 2 expressions";

            //act/assert
            var exception = Assert.ThrowsException<InvalidOperationException>(() => formatManager.GetSearchFacets(unformattedFacets));
            exception.Message.Should().Be(expectedExceptionMessage);
        }
    }
}
