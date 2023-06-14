using System;

namespace Domain.SearchResources
{
    public class Theme
    {
        public Guid ThemeId { get; set; }
        public string PrimaryHexColour { get; set; }
        public string SecondaryHexColour { get; set; }
        public string NavBarHexColour { get; set; }
        public string LogoURL { get; set; }
        public string MissingImageURL { get; set; }
    }
}
