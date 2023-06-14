using System.Collections.Generic;

namespace Domain.SearchResource
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
