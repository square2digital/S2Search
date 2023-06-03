using Domain.Models;
using Domain.Models.Facets;
using Microsoft.Extensions.Logging;
using Moq;
using Domain.Interfaces;
using Newtonsoft.Json;

namespace Tests
{
    public class TestBase
    {
        protected string _facetData = string.Empty;
        protected string _faceCacheKey = string.Empty;

        protected string GetGenericTestData
        {
            get
            {
                return @"
                {
                  ""took"": 1,
                  ""timed_out"": false,
                  ""_shards"": {
                    ""total"": 1,
                    ""successful"": 1,
                    ""skipped"": 0,
                    ""failed"": 0
                  },
                  ""hits"": {
                    ""total"": {
                      ""value"": 2,
                      ""relation"": ""eq""
                    },
                    ""max_score"": 1.0,
                    ""hits"": [
                      {
                        ""_index"": ""s2-demo-generic"",
                        ""_type"": ""_doc"",
                        ""_id"": ""92828b86-f1aa-46c9-a187-2f45059a8c19"",
                        ""_score"": 1.0,
                        ""_source"": {
                          ""Id"": ""92828b86-f1aa-46c9-a187-2f45059a8c19"",
                          ""Title"": ""Alfa Romeo Brera 3.2 V6 JTS SV Q4"",
                          ""Subtitle"": ""its a good vehicle and stuff"",
                          ""Price"": 5000,
                          ""Location"": ""Leicester"",
                          ""ImageURL"": ""https://s2storagedev.blob.core.windows.net/assets/vehicle-images/alfa-romeo_brera_1_l.jpg"",
                          ""LinkUrl"": ""https://s2storagedev.blob.core.windows.net/assets/vehicle-images/alfa-romeo_brera_1_l.jpg"",
                          ""VRM"": ""CE56OXF"",
                          ""Make"": ""Alfa Romeo"",
                          ""Model"": ""Brera"",
                          ""Variant"": ""3.2 V6 JTS SV Q4"",
                          ""MonthlyPrice"": 105,
                          ""Mileage"": 80900,
                          ""FuelType"": ""Petrol"",
                          ""Transmission"": ""Manual"",
                          ""Doors"": 2,
                          ""EngineSize"": 3200,
                          ""BodyStyle"": ""Coupe"",
                          ""Colour"": ""Silver"",
                          ""ManufactureColour"": null,
                          ""Year"": 2006,
                          ""ModelYear"": ""2006""
                        }
                      },
                      {
                        ""_index"": ""s2-demo-generic"",
                        ""_type"": ""_doc"",
                        ""_id"": ""60671dce-24d5-4d83-bfe4-e0e41415f47b"",
                        ""_score"": 1.0,
                        ""_source"": {
                          ""Id"": ""9c4af4e6-7a34-4db1-b68f-4bba7095a90d"",
                          ""Title"": ""Alfa Stelvio 2.0 Turbo 200 Sprint 5dr Auto"",
                          ""Subtitle"": ""its a good vehicle and stuff"",
                          ""Price"": 40000,
                          ""Location"": ""Warrington"",
                          ""ImageURL"": ""https://s2storagedev.blob.core.windows.net/assets/vehicle-images/alfa-romeo_stelvio_1_l.jpg"",
                          ""LinkUrl"": ""https://s2storagedev.blob.core.windows.net/assets/vehicle-images/alfa-romeo_stelvio_1_l.jpg"",
                          ""VRM"": ""PK20PKD"",
                          ""Make"": ""Alfa Romeo"",
                          ""Model"": ""Stelvio"",
                          ""Variant"": ""2.0 Turbo 200 Sprint 5dr Auto"",
                          ""MonthlyPrice"": 834,
                          ""Mileage"": 1520,
                          ""FuelType"": ""Petrol"",
                          ""Transmission"": ""Automatic"",
                          ""Doors"": 5,
                          ""EngineSize"": 2000,
                          ""BodyStyle"": ""Estate"",
                          ""Colour"": ""Grey"",
                          ""ManufactureColour"": null,
                          ""Year"": 2020,
                          ""ModelYear"": ""2020""
                        }
                      }
                    ]
                  }
                }
                ";
            }
        }

        protected string GetGenericIndexData
        {
            get
            {
                return @"                
                {
                    ""s2-demo-generic"": {
                        ""mappings"": {
                            ""dynamic"": ""true"",
                            ""properties"": {
                                ""BodyStyle"": {
                                    ""type"": ""text"",
                                    ""fields"": {
                                        ""raw"": {
                                            ""type"": ""keyword""
                                        }
                                    }
                                },
                                ""Colour"": {
                                    ""type"": ""text"",
                                    ""fields"": {
                                        ""raw"": {
                                            ""type"": ""keyword""
                                        }
                                    }
                                },
                                ""Doors"": {
                                    ""type"": ""integer""
                                },
                                ""EngineSize"": {
                                    ""type"": ""integer""
                                },
                                ""FuelType"": {
                                    ""type"": ""text"",
                                    ""fields"": {
                                        ""raw"": {
                                            ""type"": ""keyword""
                                        }
                                    }
                                },
                                ""Id"": {
                                    ""type"": ""text""
                                },
                                ""ImageURL"": {
                                    ""type"": ""text""
                                },
                                ""LinkUrl"": {
                                    ""type"": ""text""
                                },
                                ""Location"": {
                                    ""type"": ""text"",
                                    ""fields"": {
                                        ""raw"": {
                                            ""type"": ""keyword""
                                        }
                                    }
                                },
                                ""Make"": {
                                    ""type"": ""text"",
                                    ""fields"": {
                                        ""raw"": {
                                            ""type"": ""keyword""
                                        }
                                    }
                                },
                                ""Mileage"": {
                                    ""type"": ""integer""
                                },
                                ""Model"": {
                                    ""type"": ""text"",
                                    ""fields"": {
                                        ""raw"": {
                                            ""type"": ""keyword""
                                        }
                                    }
                                },
                                ""MonthlyPrice"": {
                                    ""type"": ""double""
                                },
                                ""Price"": {
                                    ""type"": ""double""
                                },
                                ""Subtitle"": {
                                    ""type"": ""text""
                                },
                                ""Title"": {
                                    ""type"": ""text""
                                },
                                ""Transmission"": {
                                    ""type"": ""text"",
                                    ""fields"": {
                                        ""raw"": {
                                            ""type"": ""keyword""
                                        }
                                    }
                                },
                                ""VRM"": {
                                    ""type"": ""text""
                                },
                                ""Variant"": {
                                    ""type"": ""text"",
                                    ""fields"": {
                                        ""raw"": {
                                            ""type"": ""keyword""
                                        }
                                    }
                                },
                                ""Year"": {
                                    ""type"": ""integer""
                                }
                            }
                        }
                    }
                }
                ";
            }
        }

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
                IList<FacetGroup> facets = JsonConvert.DeserializeObject<IList<FacetGroup>>(FacetsFromLocalFile);
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