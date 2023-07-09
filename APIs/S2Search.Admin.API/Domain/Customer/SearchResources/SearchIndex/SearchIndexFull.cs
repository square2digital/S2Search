using Domain.Customer.SearchResources.Feeds;
using Domain.Customer.SearchResources.NotificationRules;
using Domain.Customer.SearchResources.SearchInterfaces;
using Domain.Customer.SearchResources.Synonyms;

namespace Domain.Customer.SearchResources.SearchIndex
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
