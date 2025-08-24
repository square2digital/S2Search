using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Managers
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
