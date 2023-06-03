using Azure.Storage.Blobs;
using Domain.Constants;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Providers;

namespace Services.Providers
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
