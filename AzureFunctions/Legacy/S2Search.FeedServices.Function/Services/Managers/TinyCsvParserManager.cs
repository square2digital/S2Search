using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.Tokenizer.RFC4180;

namespace Services.Managers
{
    public class TinyCsvParserManager : ICsvParserManager
    {
        private readonly Options CsvOptions;
        private readonly RFC4180Tokenizer csvTokenizer;
        private readonly CsvParserOptions csvParserOptions;
        private readonly ILogger _logger;

        public TinyCsvParserManager(ILogger<TinyCsvParserManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Use a " as Quote Character, a \\ as Escape Character and a , as Delimiter.
            CsvOptions = new Options('"', '\\', ',');

            // Initialize the Rfc4180 Tokenizer:
            csvTokenizer = new RFC4180Tokenizer(CsvOptions);

            // Now Build the Parser:
            csvParserOptions = new CsvParserOptions(true, csvTokenizer);
        }

        public async Task<IEnumerable<T>> GetDataAsync<T>(Stream stream)
        {
            return await Task.Run(() => GetData<T>(stream));
        }

        public IEnumerable<T> GetData<T>(Stream stream)
        {
            var csvMapper = GetMapper<T>();
            var csvParser = GetParser(csvMapper);

            var results = csvParser.ReadFromStream(stream, Encoding.ASCII).ToList();

            var totalResults = results.Count;
            var totalErrors = results.Where(x => !x.IsValid).Count();

            if(totalErrors > 0)
            {
                foreach(var error in results.Where(x => !x.IsValid))
                {
                    _logger.LogError($"Import Error: {error.Error.Value}");
                }
            }

            _logger.LogInformation($"Total results: {totalResults}");
            _logger.LogInformation($"Total errors: {totalErrors}");
            return results.Where(x => x.IsValid).Select(y => y.Result).ToList();
        }

        private ICsvMapping<T> GetMapper<T>()
        {
            string mapperName = $"CsvMap{typeof(T).Name}";

            mapperName = $"{Assembly.GetCallingAssembly().GetName().Name}.Mappers.{mapperName}, {Assembly.GetCallingAssembly().GetName().Name}";

            Type mapper = Type.GetType(mapperName);

            if (mapper == null)
            {
                var errorMessage = $"No TinyCsvParser mapper found for type {mapperName}";
                var exception = new NotImplementedException(errorMessage);
                _logger.LogError(exception, errorMessage);
                throw exception;
            }

            object instance = Activator.CreateInstance(mapper);

            return (ICsvMapping<T>)instance;
        }

        private CsvParser<T> GetParser<T>(ICsvMapping<T> csvMapper)
        {
            return new CsvParser<T>(csvParserOptions, csvMapper);
        }
    }
}
