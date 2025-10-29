namespace S2Search.Backend.Domain.Constants;

public static class StorageQueues
{
    public const string SearchInsightsProcessing = "searchinsights-process";
    public const string PurgeCache = "cache-purge";

    public const string Process = "feed-process";
    public const string Extract = "feed-extract";
    public const string Validate = "feed-validate";
}
