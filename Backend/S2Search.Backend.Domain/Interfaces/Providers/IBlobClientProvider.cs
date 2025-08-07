using Azure.Storage.Blobs;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;

public interface IBlobClientProvider
{
    BlobClient Get(string connectionKey, string containerName, string blobName);
}
