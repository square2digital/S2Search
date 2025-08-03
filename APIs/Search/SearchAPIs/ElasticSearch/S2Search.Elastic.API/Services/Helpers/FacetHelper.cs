using Domain.Interfaces;
using Domain.Models.Facets;
using Domain.Models.Response.Generic;
using Newtonsoft.Json;

namespace Services.Helper
{
    public class FacetHelper : IFacetHelper
    {
        private readonly IAppSettings _appSettings;

        public FacetHelper(IAppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public IList<FacetGroup> GetDefaultFacetsFromLocal()
        {
            return FacetsFromLocalJSONFile();
        }

        public IList<FacetGroup> SetFacetOrder(IList<FacetGroup> facets)
        {
            return OrderFacets(facets);
        }

        private IList<FacetGroup> FacetsFromLocalJSONFile()
        {
            var json = File.ReadAllText(@"DefaultFacets.json");
            IList<FacetGroup> facets = JsonConvert.DeserializeObject<IList<FacetGroup>>(json);
            return facets;
        }

        private IList<FacetGroup> OrderFacets(IList<FacetGroup> facets)
        {
            var facetList = new List<FacetGroup>();

            foreach (var facetName in _appSettings.SearchSettings.FacetOrderList)
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

        public static SearchProductResult GetDefaultFacetsRequest
        {
            get
            {
                return JsonConvert.DeserializeObject<SearchProductResult>(GetDefaultFacetsRequestJson);
            }
        }

        public static string GetDefaultFacetsRequestJson
        {
            get
            {
                return @"
                    {
                        ""size"": 0,
                        ""sort"": {
                            ""make.raw"": ""asc""
                        },
                        ""aggs"": {
                            ""group_by_make"": {
                                ""terms"": {
                                    ""field"": ""make.raw"",
                                    ""size"": 10000
                                }
                            },
                            ""group_by_model"": {
                                ""terms"": {
                                    ""field"": ""model.raw"",
                                    ""size"": 10000
                                }
                            },
                            ""group_by_variant"": {
                                ""terms"": {
                                    ""field"": ""variant.raw"",
                                    ""size"": 10000
                                }
                            },
                            ""group_by_fuelType"": {
                                ""terms"": {
                                    ""field"": ""fuelType.raw"",
                                    ""size"": 10000
                                }
                            },
                            ""group_by_transmission"": {
                                ""terms"": {
                                    ""field"": ""transmission.raw"",
                                    ""size"": 10000
                                }
                            },
                            ""group_by_engineSize"": {
                                ""terms"": {
                                    ""field"": ""engineSize"",
                                    ""size"": 10000
                                }
                            },
                            ""group_by_colour"": {
                                ""terms"": {
                                    ""field"": ""colour.raw"",
                                    ""size"": 10000
                                }
                            },
                            ""group_by_year"": {
                                ""terms"": {
                                    ""field"": ""year"",
                                    ""size"": 10000
                                }
                            },        
                            ""group_by_doors"": {
                                ""terms"": {
                                    ""field"": ""doors"",
                                    ""size"": 10000
                                }
                            },
                            ""group_by_bodyStyle"": {
                                ""terms"": {
                                    ""field"": ""bodyStyle.raw"",
                                    ""size"": 10000
                                }
                            },
                            ""group_by_price_interval"": {
                                ""histogram"": {
                                    ""field"": ""price"",
                                    ""interval"": 5000
                                }
                            },
                            ""group_by_price_range"": {
                                ""range"": {
                                    ""field"": ""price"",
                                    ""ranges"": [
                                        {
                                            ""to"": 5000
                                        },
                                        {
                                            ""from"": 5001,
                                            ""to"": 10000
                                        },
                                        {
                                            ""from"": 10001,
                                            ""to"": 20000
                                        },
                                        {
                                            ""from"": 20001,
                                            ""to"": 30000
                                        },
                                        {
                                            ""from"": 30001,
                                            ""to"": 40000
                                        },
                                        {
                                            ""from"": 40001,
                                            ""to"": 50000
                                        },
                                        {
                                            ""from"": 50001,
                                            ""to"": 60000
                                        },                    
                                        {
                                            ""from"": 60001,
                                            ""to"": 70000
                                        },
                                        {
                                            ""from"": 70001,
                                            ""to"": 80000
                                        },
                                        {
                                            ""from"": 80001,
                                            ""to"": 90000
                                        },
                                        {
                                            ""from"": 90001,
                                            ""to"": 100000
                                        },
                                        {
                                            ""from"": 100001,
                                            ""to"": 150000
                                        },            
                                        {
                                            ""from"": 150001,
                                            ""to"": 200000
                                        },
                                        {
                                            ""from"": 200001
                                        }
                                    ]
                                }
                            },
                            ""group_by_monthlyPrice_interval"": {
                                ""histogram"": {
                                    ""field"": ""monthlyPrice"",
                                    ""interval"": 100
                                }
                            },
                            ""group_by_monthlyPrice_range"": {
                                ""range"": {
                                    ""field"": ""monthlyPrice"",
                                    ""ranges"": [
                                        {
                                            ""to"": 200
                                        },
                                        {
                                            ""from"": 201,
                                            ""to"": 400
                                        },
                                        {
                                            ""from"": 401,
                                            ""to"": 600
                                        },
                                        {
                                            ""from"": 601,
                                            ""to"": 800
                                        },
                                        {
                                            ""from"": 801,
                                            ""to"": 1000
                                        },
                                        {
                                            ""from"": 1001,
                                            ""to"": 1500
                                        },
                                        {
                                            ""from"": 1501,
                                            ""to"": 2000
                                        },
                                        {
                                            ""from"": 2001
                                        }
                                    ]
                                }
                            }        
                        }
                    }";
            }
        }
    }
}
