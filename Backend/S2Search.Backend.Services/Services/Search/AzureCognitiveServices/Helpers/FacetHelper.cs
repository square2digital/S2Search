using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Domain.Models.Facets;
using System.Net;
using System.Text.Json;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers
{
    public class FacetHelper : IFacetHelper
    {
        private readonly IAppSettings _appSettings;

        public FacetHelper(IAppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public IList<FacetGroup> GetDefaultFacetsFromOneDrive()
        {
            return FacetsFromOneDrive();
        }

        public IList<FacetGroup> GetDefaultFacetsFromLocal()
        {
            return FacetsFromLocalJSONFile();
        }

        public IList<FacetGroup> SetFacetOrder(IList<FacetGroup> facets)
        {
            return OrderFacets(facets);
        }

        private IList<FacetGroup> FacetsFromOneDrive()
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString(_appSettings.SearchSettings.DefaultFacetsURL);
                IList<FacetGroup> facets = JsonSerializer.Deserialize<IList<FacetGroup>>(json);
                return facets;
            }
        }

        private IList<FacetGroup> FacetsFromLocalJSONFile()
        {
            var json = File.ReadAllText(@"DefaultFacets.json");
            IList<FacetGroup> facets = JsonSerializer.Deserialize<IList<FacetGroup>>(json);
            return facets;
        }

        private IList<FacetGroup> OrderFacets(IList<FacetGroup> facets)
        {
            var facetList = new List<FacetGroup>();
            
            foreach(var facetName in _appSettings.SearchSettings.FacetOrderList)
            {
                FacetGroup facet = facets.Where(x => x.FacetKey == facetName).SingleOrDefault();
                if (facet != null)
                {
                    facet.FacetItems = facet.FacetItems.OrderBy(x => x.Value).ToList();
                    facetList.Add(facet);
                }
            }

            return facetList;
        }
    }
}
