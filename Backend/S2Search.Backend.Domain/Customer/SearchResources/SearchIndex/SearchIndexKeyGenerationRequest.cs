using System;
using System.Collections.Generic;

namespace S2Search.Backend.Domain.Customer.SearchResources.SearchIndex;

public class SearchIndexKeyGenerationRequest
{
    public Guid SearchIndexId { get; set; }
    public Guid CustomerId { get; set; }
    public IEnumerable<string> KeysToGenerate { get; set; }
}
