using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;
using S2Search.Backend.Domain.AzureSearch.Indexes;
using Services.Interfaces.Managers;
using Services.Interfaces.Repositories;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

namespace S2Search.Backend.Services.AzureFunctions.FeedServices.Managers
{
    public class FeedProcessingManager : IFeedProcessingManager
    {
        private readonly IAzureSearchManager azureSearchManager;
        private readonly IFeedServicesRepository feedRepo;
        private readonly IGenericSynonymRepository genericSynonymsRepo;

        public FeedProcessingManager(IAzureSearchManager azureSearchManager,
                                     IFeedServicesRepository feedRepo,
                                     IGenericSynonymRepository genericSynonymsRepo)
        {
            this.azureSearchManager = azureSearchManager ?? throw new ArgumentNullException(nameof(azureSearchManager));
            this.feedRepo = feedRepo ?? throw new ArgumentNullException(nameof(feedRepo));
            this.genericSynonymsRepo = genericSynonymsRepo ?? throw new ArgumentNullException(nameof(genericSynonymsRepo));
        }

        public Task CreateOrUpdateIndexSuggesterAsync(SearchIndexCredentials searchIndexCredentials)
        {
            var suggesterName = "vehicle-suggester";
            var suggesterFields = new[] { "make", "model", "location", "colour", "autocompleteSuggestion" };

            return azureSearchManager.CreateOrUpdateIndexWithSuggesterAsync<VehicleIndex>(searchIndexCredentials,
                                                                                         suggesterName,
                                                                                         suggesterFields);
        }

        public async Task CreateOrUpdateIndexSynonymsAsync(SearchIndexCredentials searchIndexCredentials)
        {
            var genericSynonymsCategory = "vehicles";
            var genericSynonyms = await genericSynonymsRepo.GetLatestGenericSynonymsAsync(genericSynonymsCategory);
            var synonyms = genericSynonyms.Select(x => x.SolrFormat);
            var genericSynonymsTargetFields = new[] { "make", "model" };

            await azureSearchManager.CreateOrUpdateSynonymMapAsync(searchIndexCredentials, genericSynonymsCategory, synonyms);
            await azureSearchManager.ApplySynonynMapToIndexAsync(searchIndexCredentials, genericSynonymsCategory, genericSynonymsTargetFields);
        }

        public async Task MoveCsvBlobAsync(BlobClient csvBlob, FeedBlob feedBlob)
        {
            if (csvBlob == null) throw new ArgumentNullException(nameof(csvBlob));
            if (feedBlob == null) throw new ArgumentNullException(nameof(feedBlob));

            var nextDestination = $"{feedBlob.NextDestination}/{feedBlob.FileName}";

            // get container client from blob uri
            var containerClient = new BlobContainerClient(csvBlob.Uri.GetLeftPart(UriPartial.Authority) + csvBlob.BlobContainerName, csvBlob.AccountName);

            // Using BlobClient.StartCopyFromUri requires source Uri; we'll copy then delete source
            var destBlob = containerClient.GetBlobClient(nextDestination);

            await destBlob.StartCopyFromUriAsync(csvBlob.Uri);
            await csvBlob.DeleteIfExistsAsync();
        }

        public async Task ProcessFeedDataAsync(IEnumerable<VehicleIndex> feedData, SearchIndexCredentials searchIndexCredentials)
        {
            var currentFeedDocumentsTotal = await feedRepo.GetCurrentDocumentsTotalAsync(searchIndexCredentials.SearchIndexId);

            await azureSearchManager.MergeOrUploadDocumentsAsync(feedData, searchIndexCredentials);

            if (currentFeedDocumentsTotal > 0)
            {
                var currentFeedDocumentIds = await feedRepo.GetCurrentDocumentIdsAsync(searchIndexCredentials.SearchIndexId, 1, currentFeedDocumentsTotal);
                var documentIdsToDelete = currentFeedDocumentIds.Where(x => !feedData.Any(y => y.VehicleID == x));

                await azureSearchManager.DeleteDocumentsByIdsAsync("vehicleID", documentIdsToDelete, searchIndexCredentials);
            }
        }

        public Task UpdateFeedRepositoryAsync(IEnumerable<VehicleIndex> feedData, SearchIndexCredentials searchIndexCredentials)
        {
            var newFeedDocuments = new List<NewFeedDocument>();
            newFeedDocuments.AddRange(feedData.Select(x => new NewFeedDocument()
            {
                DocumentId = x.VehicleID
            }));

            return feedRepo.MergeFeedDocumentsAsync(searchIndexCredentials.SearchIndexId, newFeedDocuments);
        }
    }
}
