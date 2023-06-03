using System;
using System.Text;

namespace Domain.Models
{
    public class SearchInsightMessage
    {
        public Guid SearchIndexId { get; set; }
        public string ActualSearchQuery { get; set; }
        public string LuceneSearchQuery { get; set; }
        public string Filters { get; set; }
        public string OrderBy { get; set; }
        public int TotalResults { get; set; }
        public DateTime DateGenerated { get; set; }

        public override string ToString()
        {
            var message = new StringBuilder();
            message.Append("Message Contents:")
                   .Append(Environment.NewLine)
                   .Append("=====================================")
                   .Append(Environment.NewLine)
                   .Append($"SearchIndexId: {SearchIndexId}")
                   .Append(Environment.NewLine)
                   .Append($"ActualSearchQuery: {ActualSearchQuery}")
                   .Append(Environment.NewLine)
                   .Append($"LuceneSearchQuery: {LuceneSearchQuery}")
                   .Append(Environment.NewLine)
                   .Append($"Filters: {Filters}")
                   .Append(Environment.NewLine)
                   .Append($"OrderBy: {OrderBy}")
                   .Append(Environment.NewLine)
                   .Append($"TotalResults: {TotalResults}")
                   .Append(Environment.NewLine)
                   .Append("=====================================");
            return message.ToString();
        }
    }
}
