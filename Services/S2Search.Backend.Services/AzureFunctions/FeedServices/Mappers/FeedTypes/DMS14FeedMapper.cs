using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.AzureFunctions.FeedServices.AzureSearch.Index;
using S2Search.Backend.Domain.AzureFunctions.FeedServices.Constants;
using S2Search.Backend.Domain.AzureSearch.Indexes;
using Services.Interfaces.Managers;
using Services.Interfaces.Mappers;

namespace S2Search.Backend.Services.AzureFunctions.FeedServices.Mappers.FeedTypes
{
    public class DMS14FeedMapper : IFeedMapper
    {
        private readonly ICsvParserManager csvParserManager;
        private readonly ILogger logger;

        public DMS14FeedMapper(ICsvParserManager csvParserManager,
                               ILogger<DMS14FeedMapper> logger)
        {
            this.csvParserManager = csvParserManager ?? throw new ArgumentNullException(nameof(csvParserManager));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string FeedDataFormat => FeedDataFormats.DMS14;

        public async Task<IEnumerable<VehicleIndex>> GetDataAsync(BlobClient csvBlob)
        {
            var vehicleIndexList = new List<VehicleIndex>();

            await using (var csvStream = await csvBlob.OpenReadAsync())
            {
                var result = await csvParserManager.GetDataAsync<DMS14>(csvStream);

                foreach (var item in result)
                {
                    vehicleIndexList.Add(item.ConvertToVehicleIndex());
                }

                if (vehicleIndexList.Count == 0)
                {
                    logger.LogWarning($"{nameof(DMS14FeedMapper)} - Failed to {nameof(GetDataAsync)}");
                }
            }

            return vehicleIndexList;
        }
    }
}
