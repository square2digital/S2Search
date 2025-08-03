using System;

namespace Domain.Customer.SearchResources.FeedCredentials
{
    public class UpdateUserStatusRequest
    {
        public Guid SearchIndexId { get; set; }
        public string Username { get; set; }
        public bool Status { get; set; }
    }
}
