using System;
using System.Collections.Generic;
using System.Text;

namespace S2Search.Backend.Domain.SearchResource;

public class ServiceResourceKey
{
    public Guid Id { get; set; }
    public Guid ServiceResourceId { get; set; }
    public string KeyType { get; set; }
    public string Name { get; set; }
    public string ApiKey { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsLatest { get; set; }
}
