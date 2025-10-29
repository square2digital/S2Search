using S2Search.Backend.Domain.Customer.SearchResources.Feeds;
using S2Search.Backend.Domain.Customer.SearchResources.SearchInterfaces;
using S2Search.Backend.Domain.Customer.SearchResources.Synonyms;
using S2Search.Backend.Domain.SearchResource;

namespace S2Search.Backend.Common.S2Search.Common.Models.SearchResource;

public class SearchResourceFull
{
    public SearchResource SearchResource { get; set; }
    public Feed Feed { get; set; }
    public IEnumerable<NotificationRule> NotificationRules { get; set; }
    public IEnumerable<Synonym> Synonyms { get; set; }
    public SearchInterface SearchInterface { get; set; }
}
