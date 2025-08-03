using System.Collections.Generic;

namespace Services.Interfaces.Managers
{
    public interface ISolrFormatConversionManager
    {
        string GetSolrFormat(string keyWord, IEnumerable<string> synonyms);
        IEnumerable<string> GetSynonyms(string solrFormat);
    }
}
