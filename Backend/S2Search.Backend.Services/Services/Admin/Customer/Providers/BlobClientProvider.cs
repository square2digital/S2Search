using Azure.Storage.Blobs;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Providers
{
    public class BlobClientProvider : IBlobClientProvider
    {
        public BlobClient Get(string connectionString, string containerName, string blobName)
        {
            return new BlobClient(connectionString, containerName, blobName);
        }
    }
}
