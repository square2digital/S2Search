using System;

namespace Domain.Models
{
    public class SearchIndexCredentials
    {
        public Guid SearchIndexId { get; set; }
        public string SearchIndexName { get; set; }
        public Guid SearchInstanceId { get; set; }
        public string SearchInstanceName { get; set; }
        public string Endpoint { get; set; }
        public string ApiKey { get; set; }
    }
}
