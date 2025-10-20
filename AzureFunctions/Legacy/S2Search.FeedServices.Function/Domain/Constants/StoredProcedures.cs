namespace Domain.Constants
{
    public static class StoredProcedures
    {
        public const string GetFeedDataFormat = "[FeedServicesFunc].[GetFeedDataFormat]";
        public const string GetCurrentFeedDocuments = "[FeedServicesFunc].[GetCurrentFeedDocuments]";
        public const string GetCurrentFeedDocumentsTotal = "[FeedServicesFunc].[GetCurrentFeedDocumentsTotal]";
        public const string MergeFeedDocuments = "[FeedServicesFunc].[MergeFeedDocuments]";

        public const string GetSearchIndexCredentials = "[FeedServicesFunc].[GetSearchIndexCredentials]";
        public const string GetSearchIndexFeedProcessingData = "[FeedServicesFunc].[GetSearchIndexFeedProcessingData]";

        public const string GetLatestGenericSynonyms = "[FeedServicesFunc].[GetLatestGenericSynonymsByCategory]";
    }
}
