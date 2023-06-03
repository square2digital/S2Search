namespace Domain.Constants
{
    public static class FeedAreas
    {
        public const string BlobMonitorDirectory = "feed-services/processing/{name}";

        public const string ProcessingDirectory = "processing";
        public const string CompletedDirectory = "completed";

        public const string Extract = "extract";
        public const string Validate = "validate";
        public const string Process = "process";

        public static readonly string ExtractDirectory = $"{ProcessingDirectory}/{Extract}";
        public static readonly string ValidateDirectory = $"{ProcessingDirectory}/{Validate}";
        public static readonly string ProcessDirectory = $"{ProcessingDirectory}/{Process}";
    }
}
