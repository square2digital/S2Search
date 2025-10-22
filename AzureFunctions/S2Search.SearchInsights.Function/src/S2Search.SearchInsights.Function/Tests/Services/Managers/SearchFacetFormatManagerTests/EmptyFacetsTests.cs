using Domain.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tests.Services.Managers.SearchFacetFormatManagerTests
{
    [TestClass]
    public class EmptyFacetsTests : TestsBase
    {
        [TestMethod]
        public void ReturnsListOfSearchFacet_WhenFacetsProvidedIsNull()
        {
            //arrange
            var unformattedFacets = (string)null;

            //act
            var result = formatManager.GetSearchFacets(unformattedFacets);

            //assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            result.Should().HaveCount(0);
            result.Should().BeOfType(typeof(List<SearchFacet>));
        }

        [TestMethod]
        public void ReturnsListOfSearchFacet_WhenFacetsProvidedIsEmptyString()
        {
            //arrange
            var unformattedFacets = "";

            //act
            var result = formatManager.GetSearchFacets(unformattedFacets);

            //assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            result.Should().HaveCount(0);
            result.Should().BeOfType(typeof(List<SearchFacet>));
        }
    }
}
