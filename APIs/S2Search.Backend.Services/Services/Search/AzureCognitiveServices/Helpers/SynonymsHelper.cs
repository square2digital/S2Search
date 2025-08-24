using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers
{
    public class SynonymsHelper : ISynonymsHelper
    {
        public IDictionary<string, List<string>> GetSynonymsDictionary(List<string> genericSynonyms)
        {
            return SplitSynonyms(genericSynonyms);
        }

        private Dictionary<string, List<string>> SplitSynonyms(List<string> synonyms)
        {
            var synonymsDict = new Dictionary<string, List<string>>();

            if (synonyms != null && synonyms.Any())
            {
                foreach (string synonym in synonyms)
                {
                    var synonymSet = synonym.Split("=>", StringSplitOptions.None);

                    var key = synonymSet[1].Trim();
                    var synonymList = synonymSet[0].Split(',').Select(x => x.Trim()).ToList();

                    synonymsDict.Add(key, synonymList);
                }
            }

            return synonymsDict;
        }
    }
}