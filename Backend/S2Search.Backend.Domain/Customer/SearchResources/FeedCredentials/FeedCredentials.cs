using System;

namespace S2Search.Backend.Domain.Customer.SearchResources.FeedCredentials;

public class FeedCredentials
{
    public Guid SearchIndexId { get; set; }
    public string SftpEndpoint { get; set; }
    public string Username { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
