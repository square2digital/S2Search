using Azure.Storage.Blobs;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;
using S2Search.Backend.Domain.Constants;
using Services.Interfaces.Managers;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;

namespace S2Search.Function.FeedServices;

public class FeedExtractor
{
    private readonly IZipArchiveValidationManager _zipValidationManager;

    public FeedExtractor(IZipArchiveValidationManager zipValidationManager)
    {
        _zipValidationManager = zipValidationManager ?? throw new ArgumentNullException(nameof(zipValidationManager));
    }

    [Function(nameof(FeedExtractor))]
    public async Task Run([QueueTrigger(StorageQueues.Extract, Connection = ConnectionStringKeys.AzureStorage)] FeedBlob feedBlob,
                                    IBinder binder,
                                    ILogger log)
    {
        log.LogInformation($"Extractor | Extracting FeedBlob - CustomerId: {feedBlob.CustomerId}");

        try
        {
            // Create a BlobClient for the source zip blob from the provided URI
            var zipBlobClient = new BlobClient(new Uri(feedBlob.BlobUri));

            // Build container base URI to construct destination blob URIs
            var sourceUri = new Uri(feedBlob.BlobUri);
            var containerName = sourceUri.Segments.Length >1 ? sourceUri.Segments[1].TrimEnd('/') : string.Empty;
            var containerUri = new Uri($"{sourceUri.Scheme}://{sourceUri.Host}/{containerName}/");

            using (var zipBlobFileStream = new MemoryStream())
            {
                await zipBlobClient.DownloadToAsync(zipBlobFileStream);
                await zipBlobFileStream.FlushAsync();
                zipBlobFileStream.Position =0;

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
                        // Compose destination blob URI and create BlobClient
                        var extractedBlobRelativePath = $"{feedBlob.NextDestination}/{firstZipEntry.FullName}".Trim('/');
                        var extractBlobUri = new Uri(containerUri, extractedBlobRelativePath);
                        var extractBlobClient = new BlobClient(extractBlobUri);

                        // Upload and overwrite if exists
                        await extractBlobClient.UploadAsync(stream, overwrite: true);
                    }
                }
            }

            log.LogInformation($"Extractor | Removing extracted FeedBlob - CustomerId: {feedBlob.CustomerId}");

            await zipBlobClient.DeleteIfExistsAsync();
        }
        catch (Exception ex)
        {
            log.LogError(ex, $"Extractor | Error: {ex.Message}");
            throw;
        }
    }
}