using System.Collections.Generic;
using System.Threading.Tasks;

namespace S2Search.Backend.Domain.Interfaces
{
    public interface ISynonymsService
    {
        Task<List<string>> GetGenericSynonyms(string category = "vehicles");
    }
}