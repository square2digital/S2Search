﻿using Domain.SearchResources.CustomerPricing;
using Domain.SearchResources.SearchIndex;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface ISearchIndexRepository
    {
        void Create(SearchIndexRequest indexRequest);
        Task CreateAsync(SearchIndexRequest indexRequest);
        Task<SearchIndex> GetAsync(Guid customerId, Guid searchIndexId);
        Task<SearchIndex> GetByFriendlyNameAsync(Guid customerId, string friendlyName);
        Task<SearchIndexFull> GetFullAsync(Guid customerId, Guid searchIndexId);
        Task<IEnumerable<CustomerPricingTier>> GetPricingTiers();
        void Delete(Guid searchIndexId);
        Task DeleteAsync(Guid searchIndexId);
        Task<IEnumerable<SearchIndexKeys>> GetKeysAsync(Guid customerId, Guid searchIndexId);
        Task CreateKeysAsync(SearchIndexKeyGenerationRequest keyGenerationRequest);
        Task DeleteKeysAsync(SearchIndexKeyDeletionRequest keyDeletionRequest);
        Task<SearchIndexQueryCredentials> GetQueryCredentials(string customerEndpoint);
    }
}
