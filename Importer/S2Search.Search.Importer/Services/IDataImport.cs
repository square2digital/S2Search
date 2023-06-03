using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace S2.Test.Importer.Services
{
    public interface IDataImport
    {
        void CleanupResources();
        void CreateVehiclesIndex();
        void UploadVehicleDocuments();
    }
}
