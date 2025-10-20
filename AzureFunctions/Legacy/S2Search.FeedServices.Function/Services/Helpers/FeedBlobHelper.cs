using Domain.Constants;
using Domain.Models;

namespace Services.Helpers
{
    public static class FeedBlobHelper
    {
        public static void SetNextDestination(this FeedBlob source)
        {
            string nextLocation = source.CurrentFeedArea switch
            {
                FeedAreas.Extract => FeedAreas.ValidateDirectory,
                FeedAreas.Validate => FeedAreas.ProcessDirectory,
                FeedAreas.Process => FeedAreas.CompletedDirectory,
                _ => "unknown",
            };

            string manualUploadPathPart = source.ManualUpload ? "/manualupload" : "";

            source.NextDestination = $"{nextLocation}/{source.CustomerId}/{source.SearchIndexName}{manualUploadPathPart}";
        }
    }
}
