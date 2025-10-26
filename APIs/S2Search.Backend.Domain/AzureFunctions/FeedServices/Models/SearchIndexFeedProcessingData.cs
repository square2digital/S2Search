namespace S2Search.Backend.Domain.AzureFunctions.FeedServices.Models
{
    public class SearchIndexFeedProcessingData
    {
        public SearchIndexCredentials SearchIndexCredentials { get; set; }
        public SearchIndexData SearchIndexData { get; set; }
    }
}
