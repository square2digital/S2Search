using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISynonymsService
    {
        Task<List<string>> GetGenericSynonyms(string callingHost, string category = "vehicles");
    }
}