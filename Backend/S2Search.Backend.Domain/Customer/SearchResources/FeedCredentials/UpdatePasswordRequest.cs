using System;

namespace S2Search.Backend.Domain.Customer.SearchResources.FeedCredentials;

public class UpdatePasswordRequest
{
    public Guid SearchIndexId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
