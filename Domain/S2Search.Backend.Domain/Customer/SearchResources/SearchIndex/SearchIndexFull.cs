using S2Search.Backend.Domain.Customer.SearchResources.Feeds;
using S2Search.Backend.Domain.Customer.SearchResources.SearchInterfaces;
using S2Search.Backend.Domain.Customer.SearchResources.Synonyms;
using S2Search.Backend.Domain.SearchResource;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchIndex;

public class SearchIndexFull
{
    public SearchIndex SearchIndex { get; set; }
    public Feed Feed { get; set; }
    public IEnumerable<NotificationRule> NotificationsRules { get; set; }
    public IEnumerable<Synonym> Synonyms { get; set; }
    public SearchInterface SearchInterface { get; set; }
}
