namespace S2.Test.Importer.Services
{
    public interface IDataImport
    {
        void CleanupResources();
        void CreateVehiclesIndex();
        void UploadVehicleDocuments();
    }
}
