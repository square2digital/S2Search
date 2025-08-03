using Azure.Storage.Blobs;

namespace Services.Interfaces.Providers
{
    public interface IBlobClientProvider
    {
        BlobClient Get(string connectionKey, string containerName, string blobName);
    }
}
