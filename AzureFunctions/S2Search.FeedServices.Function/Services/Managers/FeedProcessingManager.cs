﻿using Domain.AzureSearch.Index;
using Domain.Models;
using Microsoft.Azure.Storage.Blob;
using Services.Helpers;
using Services.Interfaces.Managers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class FeedProcessingManager : IFeedProcessingManager
    {
        private readonly IAzureSearchManager azureSearchManager;
        private readonly IFeedRepository feedRepo;
        private readonly IGenericSynonymRepository genericSynonymsRepo;

        public FeedProcessingManager(IAzureSearchManager azureSearchManager,
                                     IFeedRepository feedRepo,
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

        public Task MoveCsvBlobAsync(CloudBlockBlob csvBlob, FeedBlob feedBlob)
        {
            var nextDestination = $"{feedBlob.NextDestination}/{feedBlob.FileName}";
            return csvBlob.MoveToFolderAsync(nextDestination);
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
