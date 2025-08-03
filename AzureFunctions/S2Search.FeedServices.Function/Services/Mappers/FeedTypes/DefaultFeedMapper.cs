using Domain.AzureSearch.Index;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Interfaces.Managers;
using System;
using Domain.Constants;
using Services.Interfaces.Mappers;
using Azure.Storage.Blobs;

namespace Services.Mappers.FeedTypes
{
    public class DefaultFeedMapper : IFeedMapper
    {
        private readonly ICsvParserManager csvParserManager;
        private readonly ILogger logger;

        public DefaultFeedMapper(ICsvParserManager csvParserManager,
                                 ILogger<DefaultFeedMapper> logger)
        {
            this.csvParserManager = csvParserManager ?? throw new ArgumentNullException(nameof(csvParserManager));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string FeedDataFormat => FeedDataFormats.Default;

        public async Task<IEnumerable<VehicleIndex>> GetDataAsync(BlobClient csvBlobClient)
        {
            using (var csvStream = await csvBlobClient.OpenReadAsync())
            {
                await csvStream.FlushAsync();
                csvStream.Position = 0;

                var documents = await csvParserManager.GetDataAsync<VehicleIndex>(csvStream);

                if (documents == null || documents.Count() == 0)
                {
                    logger.LogWarning($"{nameof(DefaultFeedMapper)} - Failed to {nameof(GetDataAsync)}");
                }

                return documents;
            }
        }
    }
}
