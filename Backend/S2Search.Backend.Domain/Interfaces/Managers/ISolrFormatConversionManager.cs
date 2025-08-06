using System.Collections.Generic;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers
{
    public interface ISolrFormatConversionManager
    {
        string GetSolrFormat(string keyWord, IEnumerable<string> synonyms);
        IEnumerable<string> GetSynonyms(string solrFormat);
    }
}
