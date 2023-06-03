using Domain.AzureSearch.Index;
using Microsoft.Azure.Storage.Blob;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Mappers
{
    public interface IFeedMapper
    {
        public string FeedDataFormat { get; }
        Task<IEnumerable<VehicleIndex>> GetDataAsync(CloudBlockBlob csvBlob);
    }
}