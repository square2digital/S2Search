using System.Collections.Generic;

namespace Domain.Customer.Constants
{
    public class AcceptedFileTypes
    {
        public const string ZipFile = ".zip";
        public const string CsvFile = ".csv";

        public static readonly IEnumerable<string> List = new string[] { ZipFile, CsvFile };
        public static readonly string ListAsString = string.Join(", ", List);
    }
}
