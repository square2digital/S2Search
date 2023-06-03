using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface IGenericSynonymRepository
    {
        Task<IEnumerable<GenericSynonym>> GetLatestGenericSynonymsAsync(string category);
    }
}
