using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Managers
{
    public class SolrFormatConversionManager : ISolrFormatConversionManager
    {
        private const string solrSeperator = "=>";
        private const char synonymSeperator = ',';

        public string GetSolrFormat(string keyWord, IEnumerable<string> synonyms)
        {
            var solrFormat = string.Join(synonymSeperator.ToString(), synonyms);
            return $"{solrFormat} {solrSeperator} {keyWord}";
        }

        public IEnumerable<string> GetSynonyms(string solrFormat)
        {
            int splitterIndex = solrFormat.IndexOf(solrSeperator);
            string solrWords = solrFormat.Remove(splitterIndex).Trim();
            var synonyms = solrWords.Split(synonymSeperator).Select(word => word.Trim());
            return synonyms;
        }
    }
}
