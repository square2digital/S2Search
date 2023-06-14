using Domain.Constants;
using Domain.SearchResources.SearchInterfaces;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Managers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class SearchInterfaceRepository : ISearchInterfaceRepository
    {
        private readonly IDbContextProvider _dbContext;
        private readonly ISearchInterfaceValidationManager _searchInterfaceValidation;

        public SearchInterfaceRepository(IDbContextProvider dbContext,
                                         ISearchInterfaceValidationManager searchInterfaceValidation)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _searchInterfaceValidation = searchInterfaceValidation ?? throw new ArgumentNullException(nameof(searchInterfaceValidation));
        }

        public async Task<SearchInterface> CreateAsync(SearchInterfaceRequest searchInterfaceRequest)
        {
            if (!_searchInterfaceValidation.IsValid(searchInterfaceRequest, out string errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", searchInterfaceRequest.SearchIndexId },
                { "InterfaceType", searchInterfaceRequest.SearchInterfaceType },
                { "LogoURL", searchInterfaceRequest.LogoURL },
                { "BannerStyle", searchInterfaceRequest.BannerStyle }
            };

            await _dbContext.ExecuteAsync(ConnectionStrings.CustomerResourceStore, StoredProcedures.AddSearchInterface, parameters);

            var result = await GetLatestAsync(searchInterfaceRequest.SearchIndexId);
            return result;
        }

        public async Task<SearchInterface> GetLatestAsync(Guid SearchIndexId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "SearchIndexId", SearchIndexId }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<SearchInterface>(ConnectionStrings.CustomerResourceStore,
                                                                                     StoredProcedures.GetLatestSearchInterface,
                                                                                     parameters);
            return result;
        }
    }
}
