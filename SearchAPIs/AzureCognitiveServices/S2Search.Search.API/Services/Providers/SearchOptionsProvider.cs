using Domain.Models.Interfaces;
using Domain.Models.Request;
using Services.Interfaces;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Services.Providers
{
    public class SearchOptionsProvider : ISearchOptionsProvider
    {
        private readonly IAppSettings _appSettings;
        private readonly ISearchFilterFormatter _searchFilterFormatter;

        public SearchOptionsProvider(IAppSettings appSettings,
            ISearchFilterFormatter searchFilterFormatter)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _searchFilterFormatter = searchFilterFormatter ?? throw new ArgumentNullException(nameof(searchFilterFormatter));
        }

        public SearchOptions CreateSearchOptions(SearchRequest request)
        {
            string orderBy = request.OrderBy;

            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = _appSettings.SearchSettings.DefaultSearchOrderBy;
            }

            var filter = new List<string>();

            if (!string.IsNullOrWhiteSpace(request.Filters))
            {
                filter = request.Filters.Split(',').ToList();
            }

            SearchOptions searchOptions = new SearchOptions()
            {
                IncludeTotalCount = true,
                SearchMode = SearchMode.All,
                OrderBy = { orderBy },
                Size = request.PageSize,
                Skip = request.PageNumber == 0 ? 0 : request.NumberOfExistingResults,
                QueryType = SearchQueryType.Full,
                MinimumCoverage = 100,
                Filter = _searchFilterFormatter.Format(filter)
            };

            searchOptions.Facets.Add(_appSettings.FacetSettings.Make);
            searchOptions.Facets.Add(_appSettings.FacetSettings.Model);
            searchOptions.Facets.Add(_appSettings.FacetSettings.Location);
            searchOptions.Facets.Add(_appSettings.FacetSettings.Price);
            searchOptions.Facets.Add(_appSettings.FacetSettings.MonthlyPrice);
            searchOptions.Facets.Add(_appSettings.FacetSettings.Mileage);
            searchOptions.Facets.Add(_appSettings.FacetSettings.FuelType);
            searchOptions.Facets.Add(_appSettings.FacetSettings.Transmission);
            searchOptions.Facets.Add(_appSettings.FacetSettings.Doors);
            searchOptions.Facets.Add(_appSettings.FacetSettings.EngineSize);
            searchOptions.Facets.Add(_appSettings.FacetSettings.BodyStyle);
            searchOptions.Facets.Add(_appSettings.FacetSettings.Colour);
            searchOptions.Facets.Add(_appSettings.FacetSettings.Year);

            return searchOptions;
        }
    }
}