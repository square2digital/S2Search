using System.Collections.Generic;

namespace S2Search.Backend.Domain.Constants
{
    public static class AzureAutoSuggest
    {
        /// <summary>
        /// Name of the default suggester added to an Azure Search index
        /// </summary>
        public const string DefaultSuggesterName = "vehicle-suggester";

        /// <summary>
        /// The default fields to return in a Suggestions request
        /// </summary>
        public static IEnumerable<string> DefaultSuggestSelectFields = new[] { "make", "model", };

        /// <summary>
        /// The default fields to search against in a Suggestions request
        /// </summary>
        public static IEnumerable<string> DefaultSuggestSearchFields = new[] { "autocompleteSuggestion" };

        /// <summary>
        /// The default fields to search againt in an Autocomplete request
        /// </summary>
        public static IEnumerable<string> DefaultAutocompleteSearchFields = new[] { "make", "model", "colour", "location", "autocompleteSuggestion" };
    }
}
