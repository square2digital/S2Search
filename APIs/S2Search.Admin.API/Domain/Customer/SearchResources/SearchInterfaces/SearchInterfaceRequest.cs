using System;

namespace Domain.SearchResources.SearchInterfaces
{
    public class SearchInterfaceRequest
    {
        public Guid SearchIndexId { get; set; }
        public string SearchInterfaceType { get; set; }
        public string LogoURL { get; set; }
        public string BannerStyle { get; set; }
    }
}
