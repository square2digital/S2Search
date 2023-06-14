using System.Collections.Generic;
using System.Linq;

namespace Domain.SearchResource
{
    public class Synonym
    {
        public int SynonymId { get; set; }
        public string Key { get; set; }
        public IEnumerable<string> Words { get; private set; }

        private string _solrFormat;
        public string SolrFormat
        {
            get
            { return _solrFormat; }
            set
            {
                _solrFormat = value;
                string solrSplitter = "=>";
                int splitterIndex = _solrFormat.IndexOf(solrSplitter);
                string solrWords = _solrFormat.Remove(splitterIndex).Trim();
                Words = solrWords.Split(',').Select(word => word.Trim()).ToArray();
            }
        }
    }
}
