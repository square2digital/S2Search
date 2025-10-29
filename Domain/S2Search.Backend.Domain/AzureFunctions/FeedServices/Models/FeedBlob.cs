namespace S2Search.Backend.Domain.AzureFunctions.FeedServices.Models
{
    public class FeedBlob
    {
        public string CustomerId { get; set; }
        public string SearchIndexName { get; set; }
        public string CurrentFeedArea { get; set; }
        public string FileName { get; set; }
        public string BlobUri { get; set; }
        public string NextDestination { get; set; }
        public bool ManualUpload { get; set; }
    }
}
