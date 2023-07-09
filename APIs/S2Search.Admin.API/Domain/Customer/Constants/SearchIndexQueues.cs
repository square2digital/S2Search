using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Customer.Constants
{
    public static class SearchIndexQueues
    {
        public const string Create = "create-searchresource";
        public const string Update = "update-searchresource";
        public const string Delete = "delete-searchresource";
        public const string CreateKeys = "generate-keys";
        public const string DeleteKeys = "delete-keys";
    }
}
