using System;

namespace Domain.Models.Insights
{
    public class SearchInsightMessage
    {
        public Guid SearchIndexId { get; set; }
        public string ActualSearchQuery { get; set; }
        public string LuceneSearchQuery { get; set; }
        public string Filters { get; set; }
        public string OrderBy { get; set; }
        public int TotalResults { get; set; }
        public DateTime DateGenerated { get; } = DateTime.UtcNow;

    }
}
