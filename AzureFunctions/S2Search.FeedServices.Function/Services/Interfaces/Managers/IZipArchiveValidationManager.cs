using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace Services.Interfaces.Managers
{
    public interface IZipArchiveValidationManager
    {
        (bool, string) IsValid(ZipArchive zipArchive);
    }
}
