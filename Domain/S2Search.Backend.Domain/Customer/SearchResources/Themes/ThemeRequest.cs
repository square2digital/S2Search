using System;
using System.Collections.Generic;
using System.Text;

namespace S2Search.Backend.Domain.Customer.SearchResources.Themes;

public class ThemeRequest
{
    public Guid themeId { get; set; }
    public string primaryHexColour { get; set; }
    public string secondaryHexColour { get; set; }
    public string navBarHexColour { get; set; }
    public string logoURL { get; set; }
    public string MissingImageURL { get; set; }
}
