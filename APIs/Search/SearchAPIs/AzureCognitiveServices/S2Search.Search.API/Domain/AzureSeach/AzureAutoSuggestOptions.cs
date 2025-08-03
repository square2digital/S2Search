using Azure.Search.Documents;

namespace Domain.AzureSeach
{
    public class AzureAutoSuggestOptions
    {
        public string SuggesterName { get; set; }
        public AutocompleteOptions AutocompleteOptions { get; set; }
        public SuggestOptions SuggestOptions { get; set; }
    }
}
