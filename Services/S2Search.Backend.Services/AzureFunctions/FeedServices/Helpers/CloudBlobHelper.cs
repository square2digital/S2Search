using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace S2Search.Backend.Services.AzureFunctions.FeedServices.Helpers
{
    public static class CloudBlobHelper
    {
        /// <summary>
        /// Moves a blob to a different folder within the same container using the sourceBlob properties
        /// </summary>
        /// <param name="sourceBlob"></param>
        /// <param name="newDestination">Full path to file</param>
        /// <returns></returns>
        public static async Task MoveToFolderAsync(this BlobClient sourceBlob, string newDestination)
        {
            Validate(sourceBlob, newDestination);

            try
            {
                var container = sourceBlob.GetParentBlobContainerClient();
                var destBlob = container.GetBlobClient(newDestination);

                // start server-side copy and wait for completion
                var operation = await destBlob.StartCopyFromUriAsync(sourceBlob.Uri);
                await operation.WaitForCompletionAsync();

                // check result (some copy scenarios don't populate CopyStatus; treat None as success for server-side instantaneous copies)
                var props = await destBlob.GetPropertiesAsync();
                var status = props.Value.CopyStatus;

                if (status == Azure.Storage.Blobs.Models.CopyStatus.Success)
                {
                    await sourceBlob.DeleteIfExistsAsync();
                }
                else
                {
                    Debug.WriteLine($"Blob copy did not succeed. Status: {status}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

        private static void Validate(BlobClient sourceBlob, string newDestination)
        {
            if (sourceBlob == null)
            {
                throw new ArgumentNullException($"{nameof(sourceBlob)} cannot be null.");
            }

            if (string.IsNullOrEmpty(newDestination))
            {
                throw new ArgumentException($"{nameof(newDestination)} cannot be null or empty");
            }
        }
    }
}
