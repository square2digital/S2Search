using System;

namespace S2Search.Backend.Domain.Customer.Shared;

public class QueryKey
{
    public string Name { get; set; }
    public Guid ApiKey { get; set; }
}
