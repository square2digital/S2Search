using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Customer.SearchResources.Synonyms
{
    public class SynonymRequest
    {
        public Guid SynonymId { get; set; }
        [Required]
        public Guid SearchIndexId { get; set; }
        [Required]
        public string KeyWord { get; set; }
        [Required]
        public IEnumerable<string> Synonyms { get; set; }
    }
}
