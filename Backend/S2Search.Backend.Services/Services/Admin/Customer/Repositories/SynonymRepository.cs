using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.SearchResources.Synonyms;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Repositories
{
    public class SynonymRepository : ISynonymRepository
    {
        private readonly IDbContextProvider _dbContext;
        private readonly ISynonymValidationManager _synonymValidation;
        private readonly ISolrFormatConversionManager _solrConverter;

        public SynonymRepository(IDbContextProvider dbContext,
                                 ISynonymValidationManager synonymValidation,
                                 ISolrFormatConversionManager solrConverter)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _synonymValidation = synonymValidation ?? throw new ArgumentNullException(nameof(synonymValidation));
            _solrConverter = solrConverter ?? throw new ArgumentNullException(nameof(solrConverter));
        }

        public async Task<Synonym> CreateAsync(SynonymRequest synonymRequest)
        {
            if (!_synonymValidation.IsValid(synonymRequest, out string errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            string solrFormat = _solrConverter.GetSolrFormat(synonymRequest.KeyWord, synonymRequest.Synonyms);

            var parameters = new Dictionary<string, object>()
            {
                { "SynonymId", synonymRequest.SynonymId },
                { "SearchIndexId", synonymRequest.SearchIndexId },
                { "KeyWord", synonymRequest.KeyWord },
                { "SolrFormat", solrFormat }
            };

            var synonymId = await _dbContext.ExecuteScalarAsync<Guid>(ConnectionStrings.CustomerResourceStore,
                                                                          StoredProcedures.AddSynonym,
                                                                          parameters);

            var result = await GetByIdAsync(synonymRequest.SearchIndexId, synonymId);
            return result;
        }

        public async Task DeleteAsync(Guid SearchIndexId, Guid synonymId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", SearchIndexId },
                { "SynonymId", synonymId }
            };

            await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore, StoredProcedures.SupersedeSynonym, parameters);
        }

        public async Task<IEnumerable<Synonym>> GetAsync(Guid SearchIndexId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", SearchIndexId }
            };

            var results = await _dbContext.QueryAsync<Synonym>(ConnectionStrings.CustomerResourceStore, StoredProcedures.GetSynonyms, parameters);

            return results;
        }

        public async Task<Synonym> GetByIdAsync(Guid SearchIndexId, Guid synonymId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", SearchIndexId },
                { "SynonymId", synonymId }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<Synonym>(ConnectionStrings.CustomerResourceStore, StoredProcedures.GetSynonymById, parameters);

            return result;
        }

        public async Task<Synonym> GetByKeyWordAsync(Guid SearchIndexId, string keyWord)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", SearchIndexId },
                { "KeyWord", keyWord }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<Synonym>(ConnectionStrings.CustomerResourceStore, StoredProcedures.GetSynonymByKeyWord, parameters);

            return result;
        }

        public async Task<Synonym> UpdateAsync(SynonymRequest synonymRequest)
        {
            if (!_synonymValidation.IsValid(synonymRequest, out string errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            if(synonymRequest.SynonymId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(synonymRequest.SynonymId)} must be provided");
            }

            string solrFormat = _solrConverter.GetSolrFormat(synonymRequest.KeyWord, synonymRequest.Synonyms);

            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", synonymRequest.SearchIndexId },
                { "SynonymId", synonymRequest.SynonymId },
                { "KeyWord", synonymRequest.KeyWord },
                { "SolrFormat", solrFormat }
            };

            await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore, StoredProcedures.UpdateSynonym, parameters);

            var synonym = await GetByIdAsync(synonymRequest.SearchIndexId, synonymRequest.SynonymId);

            return synonym;
        }
    }
}
