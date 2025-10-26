using S2Search.Backend.Domain.AzureFunctions.FeedServices.Models;

namespace Services.Interfaces.Repositories
{
    public interface IGenericSynonymRepository
    {
        Task<IEnumerable<GenericSynonym>> GetLatestGenericSynonymsAsync(string category);
    }
}
