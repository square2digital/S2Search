using System.Collections.Generic;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces
{
    public interface ISynonymsHelper
    {
        IDictionary<string, List<string>> GetSynonymsDictionary(List<string> genericSynonyms);
    }
}