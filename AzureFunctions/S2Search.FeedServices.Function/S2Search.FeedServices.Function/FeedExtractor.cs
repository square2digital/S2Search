using System.IO.Compression;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Constants;
using Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;

namespace FunctionsTest
{
    public class FeedExtractor
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IZipArchiveValidationManager _zipValidationManager;
        private readonly ILogger<FeedExtractor> _logger;

        public FeedExtractor(BlobServiceClient blobServiceClient,
            IZipArchiveValidationManager zipValidationManager,
            ILogger<FeedExtractor> logger)
        {
            _blobServiceClient = blobServiceClient;
            _zipValidationManager = zipValidationManager ?? throw new ArgumentNullException(nameof(zipValidationManager));
            _logger = logger;
        }

        [Function(nameof(FeedExtractor))]
        public async Task Run([QueueTrigger(StorageQueues.Extract, Connection = ConnectionStrings.AzureStorageAccount)] FeedBlob feedBlob)
        {
            _logger.LogInformation($"Extractor | Extracting FeedBlob - CustomerId: {feedBlob.CustomerId}");

            try
            {
                // Parse the container and blob name from the BlobUri
                var blobUri = new Uri(feedBlob.BlobUri);
                var containerName = blobUri.Segments[1].TrimEnd('/'); // Extracts container name
                var blobName = string.Join("", blobUri.Segments.Skip(2)); // Extracts the blob name (after the container)

                // Initialize the BlobClient for the zip file
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var zipBlobClient = blobContainerClient.GetBlobClient(blobName);

                // Check if the blob exists
                if (!await zipBlobClient.ExistsAsync())
                {
                    _logger.LogError($"Blob does not exist: {feedBlob.BlobUri}");
                    throw new FileNotFoundException($"Blob does not exist: {feedBlob.BlobUri}");
                }

                // Download the zip blob to a memory stream
                using (var zipBlobFileStream = new MemoryStream())
                {
                    await zipBlobClient.DownloadToAsync(zipBlobFileStream);
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
                            // Define the extracted blob path
                            var extractedBlobPath = $"{feedBlob.NextDestination}/{firstZipEntry.FullName}";

                            // Create a BlobClient for the extracted file
                            var extractedBlobClient = blobContainerClient.GetBlobClient(extractedBlobPath);

                            // Upload the extracted file
                            await extractedBlobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "application/octet-stream" });
                        }
                    }
                }

                _logger.LogInformation($"Extractor | Removing extracted FeedBlob - CustomerId: {feedBlob.CustomerId}");

                // Delete the original zip file
                await zipBlobClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Extractor | Error: {ex.Message}");
                throw;
            }
        }
    }
}