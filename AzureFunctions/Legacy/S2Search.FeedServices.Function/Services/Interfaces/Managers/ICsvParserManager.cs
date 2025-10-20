using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Services.Interfaces.Managers
{
    public interface ICsvParserManager
    {
        IEnumerable<T> GetData<T>(Stream stream);
        Task<IEnumerable<T>> GetDataAsync<T>(Stream stream);
    }
}
