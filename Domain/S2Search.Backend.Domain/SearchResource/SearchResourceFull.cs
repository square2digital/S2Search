using S2Search.Backend.Domain.Customer.SearchResources.Feeds;
using S2Search.Backend.Domain.Customer.SearchResources.SearchInterfaces;
using S2Search.Backend.Domain.Customer.SearchResources.Synonyms;

namespace S2Search.Backend.Domain.SearchResource;

public class SearchResourceFull
{
    public SearchResource SearchResource { get; set; }
    public Feed Feed { get; set; }
    public IEnumerable<NotificationRule> NotificationRules { get; set; }
    public IEnumerable<Synonym> Synonyms { get; set; }
    public SearchInterface SearchInterface { get; set; }
}
