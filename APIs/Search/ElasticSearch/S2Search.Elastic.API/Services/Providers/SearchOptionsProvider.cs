using Domain.Interfaces;
using Domain.Models.Request;

namespace Services.Providers
{
    public class SearchOptionsProvider : ISearchOptionsProvider
    {
        private readonly IAppSettings _appSettings;

        public SearchOptionsProvider(IAppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public string CreateSearchOptions(SearchDataRequest request)
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

            var filterStr = SearchFilterFormatter.Format(filter);

            return filterStr;
        }
    }
}
