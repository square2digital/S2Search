using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Domain.AzureSeach;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services.Providers
{
    public class AzureAutoSuggestOptionsProvider : IAzureAutoSuggestOptionsProvider
    {
        public AzureAutoSuggestOptions Get(string suggesterName,
                                           IEnumerable<string> suggestSelectFields,
                                           IEnumerable<string> suggestSearchFields,
                                           IEnumerable<string> autocompleteSearchFields)
        {
            if (string.IsNullOrEmpty(suggesterName))
            {
                throw new ArgumentNullException(nameof(suggesterName));
            }

            var options = GetDefaultOptions(suggesterName);

            AddSuggestOptions(suggestSelectFields, suggestSearchFields, options.SuggestOptions);
            AddAutocompleteOptions(autocompleteSearchFields, options.AutocompleteOptions);

            return options;
        }

        private static AzureAutoSuggestOptions GetDefaultOptions(string suggesterName)
        {
            return new AzureAutoSuggestOptions()
            {
                SuggesterName = suggesterName,
                AutocompleteOptions = new AutocompleteOptions()
                {
                    Mode = AutocompleteMode.OneTermWithContext,
                    Size = 1
                },
                SuggestOptions = new SuggestOptions()
                {
                    Size = 100
                }
            };
        }

        private static void AddSuggestOptions(IEnumerable<string> selectFields, IEnumerable<string> searchFields, SuggestOptions suggestOptions)
        {
            if(selectFields != null)
            {
                foreach (var selectField in selectFields)
                {
                    suggestOptions.Select.Add(selectField);
                }
            }
            
            if(searchFields != null)
            {
                foreach (var searchField in searchFields)
                {
                    suggestOptions.SearchFields.Add(searchField);
                }
            }
        }

        private static void AddAutocompleteOptions(IEnumerable<string> searchFields, AutocompleteOptions autocompleteOptions)
        {
            if(searchFields != null)
            {
                foreach (var searchField in searchFields)
                {
                    autocompleteOptions.SearchFields.Add(searchField);
                }
            }
        }
    }
}
