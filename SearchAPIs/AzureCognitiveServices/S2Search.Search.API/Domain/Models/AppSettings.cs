using Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Domain.Models
{
    public class AppSettings : IAppSettings
    {
        public DemoSearchCredentials DemoSearchCredentials { get; set; }
        public SearchSettings SearchSettings { get; set; }
        public FacetSettings FacetSettings { get; set; }
        public ClientConfigurationSettings ClientConfigurationSettings { get; set; }
        public MemoryCacheSettings MemoryCacheSettings { get; set; }
        public RedisCacheSettings RedisCacheSettings { get; set; }
    }

    public class SearchSettings
    {
        public bool UseRequestedCallingHost { get; set; }
        public string DefaultFacetsURL { get; set; }
        public string DefaultSearchOrderBy { get; set; }        
        public string FacetOrder { get; set; }
        public string FacetNamesToMatch { get; set; }
        public string FacetEdgeCases { get; set; }
        public string FacetCurrencyRanges { get; set; }
        public string FacetNonCurrencyRange { get; set; }
        public string FacetToOverrideDisplay { get; set; }
        public int FacetMaxRangeToValue { get; set; }

        public IEnumerable<string> FacetNamesToMatchList
        {
            get
            {
                if(!string.IsNullOrEmpty(FacetNamesToMatch))
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    };
                    return JsonSerializer.Deserialize<List<string>>(FacetNamesToMatch, options);
                }

                return new List<string>();
            }
        }

        public IEnumerable<string> FacetOrderList
        {
            get
            {
                if (!string.IsNullOrEmpty(FacetOrder))
                {
                    return JsonSerializer.Deserialize<string[]>(FacetOrder);
                }

                return new List<string>();
            }
        }

        public IEnumerable<string> FacetCurrencyRangesList
        {
            get
            {
                return FacetCurrencyRanges.Split(',');
            }
        }

        public IEnumerable<string> FacetNonCurrencyRangeList
        {
            get
            {
                return FacetNonCurrencyRange.Split(',');
            }
        }

        public IEnumerable<string> FacetToOverrideDisplayList
        {
            get
            {
                if(FacetToOverrideDisplay.Contains(","))
                {
                    return FacetToOverrideDisplay.Split(',');
                }
                else
                {
                    var list = new List<string>
                    {
                        FacetToOverrideDisplay
                    };
                    return list;
                }
            }
        }
    }

    public class FacetSettings
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Variant { get; set; }
        public string Location { get; set; }
        public string Price { get; set; }
        public string MonthlyPrice { get; set; }
        public string Mileage { get; set; }
        public string FuelType { get; set; }
        public string Transmission { get; set; }
        public string Doors { get; set; }
        public string EngineSize { get; set; }
        public string BodyStyle { get; set; }
        public string Colour { get; set; }
        public string Year { get; set; }
    }

    public class DemoSearchCredentials
    {
        public bool UseDemoSearchCredentials { get; set; }
        public string SearchCredentialsQueryKey { get; set; }
        public Guid SearchCredentialsIndexId { get; set; }
        public string SearchCredentialsIndexName { get; set; }
        public string SearchCredentialsInstanceEndpoint { get; set; }
        public string SearchCredentialsInstanceName { get; set; }
    }

    public class ClientConfigurationSettings
    {
        public string ClientConfigurationEndpoint { get; set; }
        public string HeaderAPISubscriptionName { get; set; }
        public string APISubscriptionKey { get; set; }
    }

    public class MemoryCacheSettings
    {
        public int SearchCacheSlidingExpirySeconds { get; set; }
        public int ConfigCacheSlidingExpirySeconds { get; set; }
        public int DefaultFacetsCacheExpiryInSeconds { get; set; }
        public int GenericSynonymsCacheExpiryInSeconds { get; set; }
    }

    public class RedisCacheSettings
    {
        public string RedisConnectionString { get; set; }
        public int DefaultCacheExpiryInSeconds { get; set; }        
    }
}