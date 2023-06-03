namespace Domain.SearchResources.SearchInterfaces
{
    public class SearchInterface
    {
        public int SearchInterfaceId { get; set; }
        public string SearchEndpoint { get; set; }
        public SearchInterfaceType Type { get; set; }
        public string LogoURL { get; set; }
        public string BannerStyle { get; set; }
    }
}
