using System.Collections.Generic;
using Domain.SearchResources.Feeds;
using Domain.SearchResources.NotificationRules;
using Domain.SearchResources.SearchInterfaces;
using Domain.SearchResources.Synonyms;

namespace Domain.SearchResources.SearchIndex
{
    public class SearchIndexFull
    {
        public SearchIndex SearchIndex { get; set; }
        public Feed Feed { get; set; }
        public IEnumerable<NotificationRule> NotificationsRules { get; set; }
        public IEnumerable<Synonym> Synonyms { get; set; }
        public SearchInterface SearchInterface { get; set; }
    }
}
