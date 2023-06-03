using System;

namespace Domain.Requests
{
    public class DeleteUserRequest
    {
        public Guid SearchIndexId { get; set; }
        public string Username { get; set; }
    }
}
