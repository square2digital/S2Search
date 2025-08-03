using System;

namespace Services.Customer.Interfaces.Managers
{
    public interface IFeedUploadDestinationManager
    {
        string GetDestination(Guid customerId, string searchIndexName, bool isZipFile, bool isManualUpload);
    }
}
