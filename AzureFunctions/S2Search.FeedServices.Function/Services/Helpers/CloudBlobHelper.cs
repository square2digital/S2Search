using Microsoft.Azure.Storage.Blob;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class CloudBlobHelper
    {
        /// <summary>
        /// Moves a blob to a different folder within the same container using the sourceBlob properties
        /// </summary>
        /// <param name="sourceBlob"></param>
        /// <param name="newDestination">Full path to file</param>
        /// <returns></returns>
        public static async Task MoveToFolderAsync(this ICloudBlob sourceBlob, string newDestination)
        {
            Validate(sourceBlob, newDestination);

            try
            {
                CloudBlobClient destBlobClient = sourceBlob.Container.ServiceClient;
                CloudBlobContainer destBlobContainer = destBlobClient.GetContainerReference(sourceBlob.Container.Name);
                CloudBlockBlob destBlob = destBlobContainer.GetBlockBlobReference(newDestination);

                //todo: monitor this copy
                //instant if within same container, may be delay if across containers
                await destBlob.StartCopyAsync(sourceBlob as CloudBlockBlob);
                await sourceBlob.DeleteAsync();
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }

            return;
        }

        private static void Validate(ICloudBlob sourceBlob, string newDestination)
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
