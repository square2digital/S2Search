using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using S2.Test.Importer.Helpers;
using Azure.Search.Documents.Indexes.Models;
using S2.Importer.Providers.AzureSearch;
using System;

namespace S2.Test.Importer.Data.Synonyms
{
    public class GenerateSynonyms : IGenerateSynonyms
    {
        private readonly ILogger<GenerateSynonyms> _logger;
        private readonly AppSettings _AppSettings;
        private readonly IAzureSearchDocumentsClientProvider _searchClient;

        private string _SynonymsMapExpression = "vw, vdub=>Volkswagen\nbimma, beema, zim-zimma=>BMW";

        public GenerateSynonyms(ILoggerFactory loggerFactory,
            IOptionsSnapshot<AppSettings> appSettings,
            IAzureSearchDocumentsClientProvider searchClient)
        {
            _logger = loggerFactory.CreateLogger<GenerateSynonyms>();
            _AppSettings = appSettings.Value;
            _searchClient = searchClient;
        }

        public void UploadSynonyms()
        {
            _searchClient.GetIndexClient().CreateOrUpdateSynonymMap(CreateSynonym(_SynonymsMapExpression));
        }

        public void EnableSynonymsInVehicleIndexSafely()
        {
            int MaxNumTries = 3;

            for (int i = 0; i < MaxNumTries; ++i)
            {
                try
                {
                    SearchIndex index = _searchClient.GetIndexClient().GetIndex(_AppSettings.IndexSettings.SearchIndexName);
                    _searchClient.GetIndexClient().CreateOrUpdateIndex(index);

                    ConsoleHelper.WriteIndicatorMessage("Updated the index successfully.\n");
                    break;
                }
                catch (Exception e)
                {
                    ConsoleHelper.WriteErrorMessage($"Index update failed : {e.Message}. Attempt({i}/{MaxNumTries}).\n");
                }
            }
        }

        private SynonymMap CreateSynonym(string synonymsExpression)
        {
            var synonymMap = new SynonymMap(_AppSettings.IndexSettings.MakesSynonymsMapName, synonymsExpression);
            return synonymMap;
        }
    }
}