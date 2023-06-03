using Domain.Constants;
using Services.Interfaces.Managers;
using System;

namespace Services.Managers
{
    public class FeedUploadDestinationManager : IFeedUploadDestinationManager
    {
        public string GetDestination(Guid customerId, string searchIndexName, bool isZipFile, bool isManualUpload)
        {
            var manualPathPart = isManualUpload ? "/manualupload" : "";

            if (isZipFile)
            {
                return $"{FeedUploadDestinations.ExtractDirectory}/{customerId}/{searchIndexName}{manualPathPart}";
            }

            return $"{FeedUploadDestinations.ValidateDirectory}/{customerId}/{searchIndexName}{manualPathPart}";
        }
    }
}
