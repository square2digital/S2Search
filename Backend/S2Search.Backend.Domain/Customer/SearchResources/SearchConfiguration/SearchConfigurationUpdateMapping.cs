using System;
using System.Collections.Generic;
using System.Text;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchConfiguration;

public class SearchConfigurationUpdateMapping
{
    public Guid SearchConfigurationMappingId { get; set; }
    public Guid SeachConfigurationOptionId { get; set; }
    public Guid SearchIndexId { get; set; }
    public string Value { get; set; }
}
