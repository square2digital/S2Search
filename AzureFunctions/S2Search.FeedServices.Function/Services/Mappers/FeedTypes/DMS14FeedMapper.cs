using Domain.AzureSearch.Index;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Services.Interfaces.Managers;
using System;
using Domain.Constants;
using Services.Interfaces.Mappers;
using Azure.Storage.Blobs;

namespace Services.Mappers.FeedTypes
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

        public async Task<IEnumerable<VehicleIndex>> GetDataAsync(BlobClient csvBlobClient)
        {
            var vehicleIndexList = new List<VehicleIndex>();

            using (var csvStream = await csvBlobClient.OpenReadAsync())
            {
                await csvStream.FlushAsync();
                csvStream.Position = 0;

                var result = await csvParserManager.GetDataAsync<DMS14>(csvStream);

                foreach (var item in result)
                {
                    vehicleIndexList.Add(item.ConvertToVehicleIndex());
                }

                if (vehicleIndexList == null || vehicleIndexList.Count == 0)
                {
                    logger.LogWarning($"{nameof(DMS14FeedMapper)} - Failed to {nameof(GetDataAsync)}");
                }
            }

            return vehicleIndexList;
        }
    }
}
