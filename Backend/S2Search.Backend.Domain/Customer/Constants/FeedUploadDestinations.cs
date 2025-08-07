namespace S2Search.Backend.Domain.Customer.Constants;

public class FeedUploadDestinations
{
    /// <summary>
    /// Name of the container for feed related activities
    /// </summary>
    public const string FeedContainer = "feed-services";

    /// <summary>
    /// The root processing directory for feeds
    /// </summary>
    public const string FeedProcessingDirectory = "processing";

    /// <summary>
    /// The directory where files can be extracted
    /// </summary>
    public static readonly string ExtractDirectory = $"{FeedProcessingDirectory}/extract";

    /// <summary>
    /// The directory where files can be validated after they have been extracted
    /// </summary>
    public static readonly string ValidateDirectory = $"{FeedProcessingDirectory}/validate";
}
