using System;

namespace Domain.Requests
{
    public class CreateUserRequest
    {
        public Guid SearchIndexId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
