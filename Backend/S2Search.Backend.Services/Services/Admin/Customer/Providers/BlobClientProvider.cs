using Azure.Storage.Blobs;
using S2Search.Backend.Domain.Admin.Customer.Constants;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Providers
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
