using Domain.Exceptions;
using Domain.Indexes.Data.TestData;
using Domain.Interfaces;
using Domain.Models.Facets;
using Domain.Models.Response.Generic;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json.Linq;
using Services.Helpers;
using Services.Interfaces.FacetOverrides;
using System.Runtime.ExceptionServices;

namespace Services.Services
{
    public class ElasticIndexService : IElasticIndexService
    {
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IElasticSearchClientProvider _elasticSearchClientProvider;
        private Dictionary<string, IAggregationContainer> _aggregations;
        private List<ElasticFacet> _elasticFacets;

        public ElasticIndexService(IAppSettings appSettings,
                                    ILoggerFactory loggerFactory,
                                    IElasticSearchClientProvider elasticSearchClientProvider,
                                    IDisplayTextFormatHelper displayTextFormatHelper,
                                    IFacetHelper facetHelper,
                                    IFacetOverrideProvider facetOverrideProvider,
                                    ILogger<ElasticIndexService> logger)
        {
            _appSettings = appSettings;
            _logger = loggerFactory.CreateLogger<ElasticIndexService>();
            _elasticSearchClientProvider = elasticSearchClientProvider ?? throw new ArgumentNullException(nameof(elasticSearchClientProvider));
            _aggregations = new Dictionary<string, IAggregationContainer>();
            _elasticFacets = new List<ElasticFacet>();
        }

        public async Task<long> GetTotalIndexCount(string index)
        {
            try
            {
                var client = _elasticSearchClientProvider.GetElasticClient();
                var countResponse = await client.CountAsync<GenericResponse>(c => c.Index(index));
                return countResponse.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetTotalIndexCount)} | Message: {ex.Message}");
                throw;
            }
        }

        public bool DoesIndexExist(string index)
        {
            try
            {
                if (string.IsNullOrEmpty(index))
                {
                    throw new ArgumentNullException(nameof(index));
                }

                var client = _elasticSearchClientProvider.GetElasticClient();
                return client.Indices.Exists(index).Exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetIndexSchema)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetIndexSchema(string index)
        {
            try
            {
                if (string.IsNullOrEmpty(index))
                {
                    throw new ArgumentNullException(nameof(index));
                }

                var client = _elasticSearchClientProvider.GetElasticClient();
                var schema = await client.LowLevel.Indices.GetMappingAsync<StringResponse>(index);

                if(!schema.Success)
                {
                    throw schema.OriginalException ?? new Exception("Index schema is null ", schema.OriginalException);
                }

                return schema.Body;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetIndexSchema)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteIndex(string index)
        {
            try
            {
                if (string.IsNullOrEmpty(index))
                {
                    throw new ArgumentNullException(nameof(index));
                }

                var client = _elasticSearchClientProvider.GetElasticClient();
                var response = await client.LowLevel.Indices.DeleteAsync<StringResponse>(index);

                if (!response.Success)
                {
                    throw response.OriginalException ?? new Exception($"Error on delete index '{index}'", response.OriginalException);
                }

                return response.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(DeleteIndex)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CreateIndexFromSchemaUri(string indexName, string indexSchemaUri)
        {
            try
            {
                var indexSchema = await DownloadHelper.DownloadJson(indexSchemaUri);
                return await CreateIndex(indexName, indexSchema);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(CreateIndexFromSchemaUri)} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CreateIndex(string indexName, string indexSchemaJson)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName))
                {
                    throw new ArgumentNullException(nameof(indexName));
                }

                if (string.IsNullOrEmpty(indexSchemaJson))
                {
                    throw new ArgumentNullException(nameof(indexSchemaJson));
                }

                var client = _elasticSearchClientProvider.GetElasticClient();
                var response = await client.LowLevel.Indices.CreateAsync<StringResponse>(indexName, indexSchemaJson);

                if (!response.Success)
                {
                    throw response.OriginalException ?? new Exception($"Error on create index '{indexName}'", response.OriginalException);
                }

                return response.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(CreateIndex)} | Message: {ex.Message}");
                throw;
            }
        }

        public void ImportDataToIndex<T>(List<T> ingestData, string index) where T : class
        {
            try
            {
                DoesIndexExist(index);

                var client = _elasticSearchClientProvider.GetElasticClient();
                var seenPages = 0;
                var size = 100;

                var observableBulkAll = client.BulkAll<T>(ingestData, f => f
                    .Index(index)
                    .BackOffTime(TimeSpan.FromSeconds(10))
                    .BackOffRetries(2)
                    .RefreshOnCompleted()
                    .Size(size)
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    .BufferToBulk((descriptor, buffer) =>
                    {
                        foreach(var document in buffer)
                        {
                            descriptor.Index<T>(i => i.Document(document));
                        }
                    })
                );

                ExceptionDispatchInfo? exceptionDispatchInfo = null;

                var observer = new BulkAllObserver(
                    onNext: response =>
                    {
                        _logger.LogInformation($"Indexed {response.Page} documents");
                        Interlocked.Increment(ref seenPages);
                    },
                    onError: exception =>
                    {
                        exceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);
                    },
                    onCompleted: () =>
                    {
                        _logger.LogInformation($"Bulk indexing operation completed");
                    }
                );

                observableBulkAll.Subscribe(observer);

                if(exceptionDispatchInfo != null)
                {
                    _logger.LogError(EventIds.ElasticIngestDataError, $"Error on {seenPages} ingesting data to Elastic | Message: {exceptionDispatchInfo}");
                    exceptionDispatchInfo.Throw();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(ImportDataToIndex)} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// This will upload test data in the form of the S2 demo vehicles.
        /// the new version of S2 will support any object type and parse down to a generic format that will be used
        /// in the API responses and for the UI
        /// </summary>
        /// <param name="index"></param>
        /// <param name="demoVehiclesURL"></param>
        /// <returns></returns>
        public async Task UploadTestVehicleDocuments(string index, string demoVehiclesURL)
        {
            string JsonVehicles = string.Empty;
            List<VehicleTest> vehicles = new List<VehicleTest>();

            try
            {
                JsonVehicles = await DownloadHelper.DownloadJson(demoVehiclesURL);
                dynamic d = JObject.Parse(JsonVehicles);

                var loopCount = 0;
                foreach (var vehicle in d.vehicles)
                {
                    try
                    {
                        VehicleTest testVehicle = new VehicleTest();

                        testVehicle.vehicleID = vehicle.vehicleID;
                        testVehicle.make = vehicle.make;
                        testVehicle.make = DisplayTextFormatHelper.FirstCharToUpperAndLower(testVehicle.make);
                        testVehicle.model = vehicle.model;
                        testVehicle.model = DisplayTextFormatHelper.FirstCharToUpperAndLower(testVehicle.model);
                        testVehicle.variant = vehicle.variant;
                        testVehicle.variant = DisplayTextFormatHelper.FirstCharToUpperAndLower(testVehicle.variant);
                        
                        testVehicle.monthlyPrice = vehicle.monthlyPrice;
                        testVehicle.mileage = vehicle.mileage;
                        testVehicle.fuelType = vehicle.fuelType;
                        testVehicle.fuelType = DisplayTextFormatHelper.FirstCharToUpperAndLower(testVehicle.fuelType);
                        testVehicle.transmission = vehicle.transmission;
                        testVehicle.transmission = DisplayTextFormatHelper.FirstCharToUpperAndLower(testVehicle.transmission);
                        testVehicle.doors = vehicle.doors;
                        testVehicle.engineSize = (int)vehicle.engineSize;
                        testVehicle.bodyStyle = vehicle.bodyStyle;
                        testVehicle.bodyStyle = DisplayTextFormatHelper.FirstCharToUpperAndLower(testVehicle.bodyStyle);
                        testVehicle.colour = vehicle.colour;
                        testVehicle.colour = DisplayTextFormatHelper.FirstCharToUpperAndLower(testVehicle.colour);
                        testVehicle.vrm = vehicle.vRM;
                        testVehicle.year = vehicle.year;
                        testVehicle.imageURL = vehicle.imageURL;

                        // Generic Product Properties
                        testVehicle.id = Guid.NewGuid().ToString();
                        testVehicle.title = $"{testVehicle.make} {testVehicle.model} {testVehicle.variant}";
                        testVehicle.subtitle = $"{testVehicle.year} {testVehicle.colour} {testVehicle.bodyStyle}";
                        testVehicle.price = vehicle.price;
                        testVehicle.city = vehicle.location;
                        testVehicle.imageUrl = vehicle.ImageUrl;
                        testVehicle.linkUrl = vehicle.LinkUrl;

                        vehicles.Add(testVehicle);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error on {nameof(UploadTestVehicleDocuments)} | loopCount = {loopCount + 1} | Message: {ex.Message}");
                        continue;
                    }
                }

                ImportDataToIndex(vehicles, index);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(UploadTestVehicleDocuments)} | Message: {ex.Message}");
                throw;
            }
        }
    }
}