namespace S2Search.Backend.Domain.SearchResource
{
    public class SearchInterface
    {
        public int SearchInterfaceId { get; set; }
        public SearchInterfaceType Type { get; set; }
        public string LogoURL { get; set; }
        public string BannerStyle { get; set; }
    }
}
