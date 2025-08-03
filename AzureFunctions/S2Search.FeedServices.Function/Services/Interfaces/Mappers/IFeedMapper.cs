using Azure.Storage.Blobs;
using Domain.AzureSearch.Index;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Mappers
{
    public interface IFeedMapper
    {
        public string FeedDataFormat { get; }
        Task<IEnumerable<VehicleIndex>> GetDataAsync(BlobClient csvBlob);
    }
}