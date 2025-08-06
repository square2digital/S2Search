using System;

namespace S2Search.Backend.Domain.Configuration.SearchResources
{
    public class Theme
    {
        public Guid ThemeId { get; set; }
        public string PrimaryHexColour { get; set; }
        public string SecondaryHexColour { get; set; }
        public string NavBarHexColour { get; set; }
        public string LogoURL { get; set; }
        public string MissingImageURL { get; set; }
        public Guid CustomerId { get; set; }
        public Guid SearchIndexId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
