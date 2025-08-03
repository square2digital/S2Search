﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Customer.SearchResources.FeedCredentials
{
    public class CreateUserRequest
    {
        public Guid SearchIndexId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
