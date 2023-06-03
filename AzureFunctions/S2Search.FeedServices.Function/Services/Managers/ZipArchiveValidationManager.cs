using Domain.Constants;
using Services.Interfaces.Managers;
using System;
using System.IO.Compression;

namespace Services.Managers
{
    public class ZipArchiveValidationManager : IZipArchiveValidationManager
    {
        public (bool, string) IsValid(ZipArchive zipArchive)
        {
            if (zipArchive.Entries.Count == 0)
            {
                return (false, "No files detected");
            }

            if (zipArchive.Entries.Count > 1)
            {
                return (false, $"Multiple files detected, zip must only contain a single {AcceptedFileTypes.CsvFile} file");
            }

            var firstZipEntry = zipArchive.Entries[0];

            if (!firstZipEntry.FullName.EndsWith(AcceptedFileTypes.CsvFile, StringComparison.OrdinalIgnoreCase))
            {
                return (false, $"The contents of the zip file must be a {AcceptedFileTypes.CsvFile} file");
            }

            if (firstZipEntry.Length == 0)
            {
                return (false, $"File has no content");
            }

            return (true, "");
        }
    }
}
