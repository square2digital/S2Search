using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.SearchResources.SearchConfiguration
{
    public class SearchConfigurationUpdateMapping
    {
        public Guid SearchConfigurationMappingId { get; set; }
        public Guid SeachConfigurationOptionId { get; set; }
        public Guid SearchIndexId { get; set; }
        public string Value { get; set; }
    }
}
