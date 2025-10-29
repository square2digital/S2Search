using Azure.Storage.Blobs;
using S2Search.Backend.Domain.AzureSearch.Indexes;

namespace Services.Interfaces.Mappers
{
    public interface IFeedMapper
    {
        public string FeedDataFormat { get; }
        Task<IEnumerable<VehicleIndex>> GetDataAsync(BlobClient csvBlob);
    }
}