using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Collections.Generic;
using S2.Test.Importer.Data;
using System.Threading;
using S2.Test.Importer.Helpers;
using S2.Importer.Providers.AzureSearch;
using Azure.Search.Documents;
using Newtonsoft.Json.Linq;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Azure;
using Azure.Search.Documents.Indexes;

namespace S2.Test.Importer.Services
{
    public class DataImport : IDataImport
    {
        private readonly ILogger<DataImport> _logger;
        private readonly AppSettings _AppSettings;
        private readonly IAzureSearchDocumentsClientProvider _searchClientProvider;

        public DataImport(ILoggerFactory loggerFactory,
            IOptionsSnapshot<AppSettings> appSettings,
            IAzureSearchDocumentsClientProvider searchClientProvider)
        {
            _logger = loggerFactory.CreateLogger<DataImport>();
            _AppSettings = appSettings.Value;
            _searchClientProvider = searchClientProvider;
        }

        public void CleanupResources()
        {
            try
            {
                _searchClientProvider.GetIndexClient().DeleteIndex(_AppSettings.IndexSettings.SearchIndexName);
                _searchClientProvider.GetIndexClient().DeleteSynonymMap(_AppSettings.IndexSettings.MakesSynonymsMapName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                ConsoleHelper.WriteInformationMessage(ex.ToString());
            }
        }

        public void CreateVehiclesIndex()
        {
            try
            {
                SearchIndex indexDefinition = new SearchIndex(_AppSettings.IndexSettings.SearchIndexName);
                indexDefinition.Fields = new FieldBuilder().Build(typeof(Vehicle));
                _searchClientProvider.GetIndexClient().CreateIndex(indexDefinition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                ConsoleHelper.WriteInformationMessage(ex.ToString());
                throw;
            }
        }

        public void UploadVehicleDocuments()
        {
            string JsonVehicles = string.Empty;
            List<Vehicle> vehicles = new List<Vehicle>();

            using (var webClient = new WebClient())
            {
                JsonVehicles = webClient.DownloadString(_AppSettings.SearchSettings.DemoVehiclesURL);
            }

            dynamic d = JObject.Parse(JsonVehicles);

            foreach (var vehicle in d.Vehicles)
            {
                try
                {
                    Vehicle SearchVehicle = new Vehicle();

                    SearchVehicle.VehicleID = vehicle.VehicleID;
                    SearchVehicle.Make = vehicle.Make;
                    SearchVehicle.Make = StringHelper.FirstCharToUpperAndLower(SearchVehicle.Make);
                    SearchVehicle.Model = vehicle.Model;
                    SearchVehicle.Model = StringHelper.FirstCharToUpperAndLower(SearchVehicle.Model);
                    SearchVehicle.Variant = vehicle.Variant;
                    SearchVehicle.Variant = StringHelper.FirstCharToUpperAndLower(SearchVehicle.Variant);
                    SearchVehicle.Location = vehicle.Location;
                    SearchVehicle.Location = StringHelper.FirstCharToUpperAndLower(SearchVehicle.Location);

                    SearchVehicle.Price = vehicle.Price;
                    SearchVehicle.MonlthyPrice = vehicle.MonlthyPrice;
                    SearchVehicle.Mileage = vehicle.Mileage;
                    SearchVehicle.FuelType = vehicle.FuelType;
                    SearchVehicle.FuelType = StringHelper.FirstCharToUpperAndLower(SearchVehicle.FuelType);
                    SearchVehicle.Transmission = vehicle.Transmission;
                    SearchVehicle.Transmission = StringHelper.FirstCharToUpperAndLower(SearchVehicle.Transmission);
                    SearchVehicle.Doors = vehicle.Doors;
                    SearchVehicle.EngineSize = (int)vehicle.EngineSize;
                    SearchVehicle.BodyStyle = vehicle.BodyStyle;
                    SearchVehicle.BodyStyle = StringHelper.FirstCharToUpperAndLower(SearchVehicle.BodyStyle);
                    SearchVehicle.ManufactureColour = vehicle.ManufactureColour;
                    SearchVehicle.ManufactureColour = StringHelper.FirstCharToUpperAndLower(SearchVehicle.ManufactureColour);
                    SearchVehicle.Colour = vehicle.Colour;
                    SearchVehicle.Colour = StringHelper.FirstCharToUpperAndLower(SearchVehicle.Colour);
                    SearchVehicle.VRM = vehicle.VRM;
                    SearchVehicle.Year = vehicle.Year;
                    SearchVehicle.ImageURL = vehicle.ImageURL;

                    vehicles.Add(SearchVehicle);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteInformationMessage($"Failed to create Vehicle documents: ID = {vehicle.ID} Make = {vehicle.Make} Model = {vehicle.Model} Reg = {vehicle.VRM}");
                    ConsoleHelper.WriteErrorMessage(ex.ToString());
                }
            }

            try
            {
                IndexDocumentsBatch<Vehicle> batch = IndexDocumentsBatch.MergeOrUpload<Vehicle>(vehicles);
                IndexDocumentsOptions options = new IndexDocumentsOptions { ThrowOnAnyError = true };
                _searchClientProvider.GetSearchClient().IndexDocuments(batch, options);
            }
            catch (RequestFailedException e)
            {
                // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                // the batch. Depending on your application, you can take compensating actions like delaying and
                // retrying. For this simple demo, we just log the failed document keys and continue.
                ConsoleHelper.WriteErrorMessage($"Failed to index some of the documents: TODO {e.Message}");
                throw;
            }

            ConsoleHelper.WriteIndicatorMessage("Waiting for documents to be indexed...\n");
            Thread.Sleep(2000);
        }
    }
}