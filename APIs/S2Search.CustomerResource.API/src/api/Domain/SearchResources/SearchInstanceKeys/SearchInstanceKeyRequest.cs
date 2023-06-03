using Domain.Enums;

namespace Domain.SearchResources.SearchInstanceKeys
{
    public class SearchInstanceKeyRequest
    {
        public string Name { get; set; }
        public SearchInstanceKeyType Type { get; set; }
    }
}
