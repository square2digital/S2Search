using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Domain.Constants;
using Domain.Models;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;

namespace S2Search.FeedServices.Function
{
    public class FeedExtractor
    {
        private readonly IZipArchiveValidationManager _zipValidationManager;

        public FeedExtractor(IZipArchiveValidationManager zipValidationManager)
        {
            _zipValidationManager = zipValidationManager ?? throw new ArgumentNullException(nameof(zipValidationManager));
        }

        [FunctionName(FunctionNames.FeedExtractor)]
        public async Task Run([QueueTrigger(StorageQueues.Extract, Connection = ConnectionStrings.AzureStorageAccount)] FeedBlob feedBlob,
                                    IBinder binder,
                                    ILogger log)
        {
            log.LogInformation($"Extractor | Extracting FeedBlob - CustomerId: {feedBlob.CustomerId}");

            try
            {
                var zipBlob = await binder.BindAsync<CloudBlockBlob>(new BlobAttribute(feedBlob.BlobUri));
                string extractedBlobPath = $"{zipBlob.Container.Uri}/{feedBlob.NextDestination}";

                using (var zipBlobFileStream = new MemoryStream())
                {
                    await zipBlob.DownloadToStreamAsync(zipBlobFileStream);
                    await zipBlobFileStream.FlushAsync();
                    zipBlobFileStream.Position = 0;

                    using (var zipFile = new ZipArchive(zipBlobFileStream))
                    {
                        var (isValid, errorMessage) = _zipValidationManager.IsValid(zipFile);

                        if (!isValid)
                        {
                            throw new Exception($"ZipArchive Validation Error - {errorMessage}");
                        }

                        var firstZipEntry = zipFile.Entries[0];

                        using (var stream = firstZipEntry.Open())
                        {
                            var extractBlob = await binder.BindAsync<CloudBlockBlob>(new BlobAttribute($"{extractedBlobPath}/{firstZipEntry.FullName}"));
                            extractBlob.UploadFromStream(stream);
                        }
                    }
                }

                log.LogInformation($"Extractor | Removing extracted FeedBlob - CustomerId: {feedBlob.CustomerId}");

                await zipBlob.DeleteAsync();
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Extractor | Error: {ex.Message}");
                throw;
            }
        }
    }
}
