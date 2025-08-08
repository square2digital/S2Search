using S2Search.Backend.Domain.Configuration.SearchResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace S2Search.Backend.Domain.Customer.SearchResources.Themes;

public class ThemeCollection
{
    public IEnumerable<Theme> Themes { get; set; }
}
