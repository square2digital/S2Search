using Azure.Search.Documents;

namespace S2Search.Backend.Domain.AzureSearch
{
    public class AzureAutoSuggestOptions
    {
        public string SuggesterName { get; set; }
        public AutocompleteOptions AutocompleteOptions { get; set; }
        public SuggestOptions SuggestOptions { get; set; }
    }
}
