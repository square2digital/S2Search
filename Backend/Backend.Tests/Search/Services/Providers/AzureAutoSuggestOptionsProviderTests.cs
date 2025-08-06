using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Interfaces;
using Services.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace S2Search.Backend.Tests.Search.Services.Providers
{
    [TestClass]
    public class AzureAutoSuggestOptionsProviderTests
    {
        private IAzureAutoSuggestOptionsProvider _systemUnderTest;

        [TestInitialize]
        public void Setup()
        {
            _systemUnderTest = new AzureAutoSuggestOptionsProvider();
        }

        [TestMethod]
        public void GetDefaultOptionsIsNotNullWithFieldsSupplied()
        {
            //arrange
            var suggesterName = "test";
            var suggestSelectFields = new[] { "test" };
            var suggestSearchFields = new[] { "test" };
            var autocompleteSearchFields = new[] { "test" };


            //act
            var options = _systemUnderTest.Get(suggesterName, suggestSelectFields, suggestSearchFields, autocompleteSearchFields);

            //assert
            Assert.IsNotNull(options);
        }

        [TestMethod]
        public void GetDefaultOptionsIsNotNullWithoutFieldsSupplied()
        {
            //arrange
            var suggesterName = "test";

            //act
            var options = _systemUnderTest.Get(suggesterName, null, null, null);

            //assert
            Assert.IsNotNull(options);
        }

        [TestMethod]
        public void GetDefaultOptionsThrowsExceptionWhenSuggesterNameNotSupplied()
        {
            //arrange
            var suggestSelectFields = new[] { "test" };
            var suggestSearchFields = new[] { "test" };
            var autocompleteSearchFields = new[] { "test" };

            //act/assert
            Assert.ThrowsException<ArgumentNullException>(() => _systemUnderTest.Get(null, suggestSelectFields, suggestSearchFields, autocompleteSearchFields));
            Assert.ThrowsException<ArgumentNullException>(() => _systemUnderTest.Get(null, null, null, null));
        }

        [TestMethod]
        public void GetDefaultOptionsHasCorrectOptions()
        {
            //arrange
            var suggesterName = "test";
            var suggestSelectFields = new[] { "test" };
            var suggestSearchFields = new[] { "test" };
            var autocompleteSearchFields = new[] { "test" };

            //act
            var options = _systemUnderTest.Get(suggesterName, suggestSelectFields, suggestSearchFields, autocompleteSearchFields);

            //assert
            Assert.IsTrue(options.AutocompleteOptions.Mode == Azure.Search.Documents.Models.AutocompleteMode.OneTermWithContext, "Autocomplete.Mode incorrect for AutoSuggest setup");
            Assert.IsTrue(options.AutocompleteOptions.Size == 1, "AutocompleteOptions.Size incorrect for AutoSuggest setup");
            Assert.IsTrue(options.SuggestOptions.Size == 100, "SuggestOptions.Size incorrect for AutoSuggest setup");
        }

        [TestMethod]
        [DataRow(new[] { "test" }, new[] { "test" }, new[] { "test" })]
        [DataRow(new[] { "test1", "test2" }, new[] { "test1", "test2" }, new[] { "test1", "test2" })]
        [DataRow(new[] { "test1", "test2", "test3" }, new[] { "test1", "test2", "test3" }, new[] { "test1", "test2", "test3" })]
        [DataRow(new[] { "test1" }, new[] { "test1", "test2" }, new[] { "test1", "test2", "test3" })]
        [DataRow(null, new[] { "test1", "test2" }, new[] { "test1", "test2", "test3" })]
        [DataRow(new[] { "test1" }, null, new[] { "test1", "test2", "test3" })]
        [DataRow(new[] { "test1" }, new[] { "test1", "test2" }, null)]
        public void GetDefaultOptionsCorrectNumberOfSuggestSelectFieldsAreAdded(IEnumerable<string> suggestSelectFields,
                                                                                IEnumerable<string> suggestSearchFields,
                                                                                IEnumerable<string> autocompleteSearchFields)
        {
            //arrange
            int expectedSuggestSelectFieldsCount = suggestSelectFields?.Count() ?? 0;
            int expectedSuggestSearchFieldsCount = suggestSearchFields?.Count() ?? 0;
            int expectedAutocompleteSearchFieldsCount = autocompleteSearchFields?.Count() ?? 0;


            //act
            var options = _systemUnderTest.Get("test", suggestSelectFields, suggestSearchFields, autocompleteSearchFields);

            //assert
            Assert.IsTrue(options.SuggestOptions.Select.Count == expectedSuggestSelectFieldsCount,
                            $"Actual count '{options.SuggestOptions.Select.Count}', Expected count '{suggestSelectFields?.Count() ?? 0}'");
            Assert.IsTrue(options.SuggestOptions.SearchFields.Count == expectedSuggestSearchFieldsCount, 
                            $"Actual count '{options.SuggestOptions.SearchFields.Count}', Expected count '{suggestSearchFields?.Count() ?? 0}'");
            Assert.IsTrue(options.AutocompleteOptions.SearchFields.Count == expectedAutocompleteSearchFieldsCount, 
                            $"Actual count '{options.AutocompleteOptions.SearchFields.Count}', Expected count '{autocompleteSearchFields?.Count() ?? 0}'");
        }
    }
}
