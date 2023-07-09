using System;

namespace Domain.Customer.SearchResources.FeedCredentials
{
    public class DeleteUserRequest
    {
        public Guid SearchIndexId { get; set; }
        public string Username { get; set; }
    }
}
