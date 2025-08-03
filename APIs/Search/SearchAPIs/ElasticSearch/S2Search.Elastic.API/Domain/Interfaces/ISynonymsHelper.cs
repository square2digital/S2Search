using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ISynonymsHelper
    {
        IDictionary<string, List<string>> GetSynonymsDictionary(List<string> genericSynonyms);
    }
}