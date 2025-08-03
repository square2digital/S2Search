using System.Collections.Generic;
using System.IO;
using Domain.Models;
using Domain.Models.Facets;
using Domain.Models.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Interfaces;
using System.Text.Json;

namespace Tests
{
    public class TestBase
    {
        protected string _facetData = string.Empty;
        protected string _faceCacheKey = string.Empty;

        protected string FacetsFromLocalFile
        {
            get
            {
                if (string.IsNullOrEmpty(_facetData))
                {
                    _facetData = File.ReadAllText(@"DefaultFacets.json");
                }

                return _facetData;
            }
        }

        protected Dictionary<string, List<string>> GetSynonymsDictionary
        {
            get
            {
                var SynonymsDict = new Dictionary<string, List<string>>();

                SynonymsDict.Add("Volkswagen", new List<string>() { "vw", "vdub" });
                SynonymsDict.Add("BMW", new List<string>() { "bimma", "beema", "zim-zimma" });

                return SynonymsDict;
            }
        }

        protected List<string> GetSynonymsList
        {
            get
            {
                var SynonymsList = new List<string>();

                SynonymsList.Add("beema, bimma => BMW");
                SynonymsList.Add("VW => Volkswagen");

                return SynonymsList;
            }
        }

        protected Mock<IAppSettings> GetAppsettigsMock
        {
            get
            {
                var AppsettigsMock = new Mock<IAppSettings>();
                var AppSettings = GetSearchSettings;
                AppsettigsMock.Setup(x => x.SearchSettings).Returns(AppSettings.SearchSettings);
                AppsettigsMock.Setup(x => x.MemoryCacheSettings).Returns(AppSettings.MemoryCacheSettings);

                return AppsettigsMock;
            }
        }

        protected Mock<IFacetHelper> GetFacetHelperMock
        {
            get
            {
                var FacetHelperMock = new Mock<IFacetHelper>();
                IList<FacetGroup> facets = JsonSerializer.Deserialize<IList<FacetGroup>>(FacetsFromLocalFile);
                FacetHelperMock.Setup(x => x.GetDefaultFacetsFromLocal()).Returns(facets);

                return FacetHelperMock;
            }
        }

        protected Mock<ILoggerFactory> GetLoggerMock
        {
            get
            {
                var loggerMock = new Mock<ILogger>();
                var loggerFactoryMock = new Mock<ILoggerFactory>();
                loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

                return loggerFactoryMock;
            }
        }

        protected FacetItem GetTestPorscheEngineSizeFacetItem
        {
            get
            {
                FacetItem facet = new FacetItem();

                facet.Value = "2893";
                facet.Type = "Value";
                facet.From = null;
                facet.To = null;
                facet.Count = 2;
                facet.FacetDisplayText = "2.9 L";
                facet.Selected = false;

                return facet;
            }
        }

        private AppSettings GetSearchSettings
        {
            get
            {
                AppSettings appSettings = new AppSettings();

                appSettings.SearchSettings = new SearchSettings()
                {
                    DefaultSearchOrderBy = "price desc",
                    DefaultFacetsURL = "https://onedrive.live.com/download?cid=7B0A3FF24BCC6D53&resid=7B0A3FF24BCC6D53%21104076&authkey=AMKXfQvrpdAZyc8",
                    FacetNamesToMatch = "[\"make\", \"model\", \"transmission\", \"fuelType\", \"year\", \"colour\", \"bodyStyle\" ]",
                    FacetEdgeCases = "[{\"SearchTerm\": \"6\", \"FullSearchTerm\": \"bmw\", \"Result\":true}, {\"SearchTerm\": \"6\", \"FullSearchTerm\": \"mazda\", \"Result\":false}, {\"SearchTerm\": \"6\", \"FullSearchTerm\": \"6 door\", \"Result\":true}, {\"SearchTerm\": \"6\", \"FullSearchTerm\": \"6 doors\", \"Result\":true}]",
                    FacetCurrencyRanges = "price,monthlyPrice",
                    FacetNonCurrencyRange = "mileage,engineSize",
                    FacetMaxRangeToValue = 20000000
                };

                appSettings.MemoryCacheSettings = new MemoryCacheSettings()
                {
                    SearchCacheSlidingExpirySeconds = 1,
                    ConfigCacheSlidingExpirySeconds = 1,
                    DefaultFacetsCacheExpiryInSeconds = 60
                };

                return appSettings;
            }
        }
    }
}