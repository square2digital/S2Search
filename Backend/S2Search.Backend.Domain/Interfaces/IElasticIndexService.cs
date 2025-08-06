namespace S2Search.Backend.Domain.Interfaces
{
    public interface IElasticIndexService
    {
        Task<long> GetTotalIndexCount(string index);
        Task<string> GetIndexSchema(string index);
        Task<bool> DeleteIndex(string index);
        Task<bool> CreateIndexFromSchemaUri(string indexName, string indexSchemaUri);
        Task<bool> CreateIndex(string indexName, string indexSchemaJson);
        bool DoesIndexExist(string index);
        void ImportDataToIndex<T>(List<T> ingestData, string index) where T : class;
        Task UploadTestVehicleDocuments(string index, string demoVehiclesURL);
    }
}