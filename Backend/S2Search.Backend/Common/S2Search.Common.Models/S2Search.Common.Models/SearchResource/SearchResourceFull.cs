using System.Collections.Generic;

namespace S2Search.Backend.Common.S2Search.Common.Models.S2Search.Common.Models.SearchResource
{
    public class SearchResourceFull
    {
        public SearchResource SearchResource { get; set; }
        public Feed Feed { get; set; }
        public IEnumerable<NotificationRule> NotificationRules { get; set; }
        public IEnumerable<Synonym> Synonyms { get; set; }
        public SearchInterface SearchInterface { get; set; }
    }
}
