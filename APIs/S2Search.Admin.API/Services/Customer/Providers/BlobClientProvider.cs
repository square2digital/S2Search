using Azure.Storage.Blobs;
using Domain.Customer.Constants;
using Services.Customer.Interfaces.Providers;
using Services.Dapper.Interfaces.Providers;

namespace Services.Customer.Providers
{
    public class BlobClientProvider : IBlobClientProvider
    {
        private readonly IConnectionStringProvider _connectionString;

        public BlobClientProvider(IConnectionStringProvider connectionString)
        {
            _connectionString = connectionString;
        }

        public BlobClient Get(string connectionKey, string containerName, string blobName)
        {
            return new BlobClient(_connectionString.Get(ConnectionStrings.BlobStorage), containerName, blobName);
        }
    }
}
