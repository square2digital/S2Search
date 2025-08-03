using Domain.Customer.Enums;

namespace Domain.Customer.SearchResources.SearchInstanceKeys
{
    public class SearchInstanceKeyRequest
    {
        public string Name { get; set; }
        public SearchInstanceKeyType Type { get; set; }
    }
}
