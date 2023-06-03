using Domain.AzureSearch.Index;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Services.Interfaces.Managers;
using System;
using Domain.Constants;
using Services.Interfaces.Mappers;

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

        public async Task<IEnumerable<VehicleIndex>> GetDataAsync(CloudBlockBlob csvBlob)
        {
            using (var csvStream = new MemoryStream())
            {
                await csvBlob.DownloadToStreamAsync(csvStream);
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
