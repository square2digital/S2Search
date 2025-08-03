using Domain.Constants;
using Microsoft.AspNetCore.Http;
using Services.Interfaces.Managers;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class FeedUploadValidationManager : IFeedUploadValidationManager
    {
        public async Task<(bool, string)> IsValid(IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                return (false, $"{nameof(file)} is null or has no content");
            }

            var isAcceptedType = AcceptedFileTypes.List.Any(x => file.FileName.EndsWith(x, StringComparison.OrdinalIgnoreCase));

            if (!isAcceptedType)
            {
                return (false, $"{nameof(file)} type must be one of the following: {AcceptedFileTypes.ListAsString}");
            }

            var isZip = file.FileName.EndsWith(AcceptedFileTypes.ZipFile, StringComparison.OrdinalIgnoreCase);

            if (isZip)
            {
                using (var zipBlobFileStream = new MemoryStream())
                {
                    await file.CopyToAsync(zipBlobFileStream);
                    await zipBlobFileStream.FlushAsync();
                    zipBlobFileStream.Position = 0;

                    using (var zipFile = new ZipArchive(zipBlobFileStream))
                    {
                        if(zipFile.Entries.Count == 0)
                        {
                            return (false, "No files detected");
                        }

                        if(zipFile.Entries.Count > 1)
                        {
                            return (false, $"Multiple files detected, zip must only contain a single {AcceptedFileTypes.CsvFile} file");
                        }

                        var zipEntry = zipFile.Entries[0];

                        if (!zipEntry.FullName.EndsWith(AcceptedFileTypes.CsvFile, StringComparison.OrdinalIgnoreCase))
                        {
                            return (false, $"Zip file must be a {AcceptedFileTypes.CsvFile} file");
                        }

                        if(zipEntry.Length == 0)
                        {
                            return (false, $"File has no content");
                        }
                    }
                }
            }

            return (true, "");
        }
    }
}
