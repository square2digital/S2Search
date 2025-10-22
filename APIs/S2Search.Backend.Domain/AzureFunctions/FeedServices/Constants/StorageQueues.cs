namespace S2Search.Backend.Domain.AzureFunctions.FeedServices.Constants
{
    public static class StorageQueues
    {
        public const string Process = "feed-process";
        public const string Extract = "feed-extract";
        public const string Validate = "feed-validate";
        public const string PurgeCache = "cache-purge";
    }
}
