using System;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

public interface IFeedUploadDestinationManager
{
    string GetDestination(Guid customerId, string searchIndexName, bool isZipFile, bool isManualUpload);
}
