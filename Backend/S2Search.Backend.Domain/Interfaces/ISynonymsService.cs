namespace S2Search.Backend.Domain.Interfaces
{
    public interface ISynonymsService
    {
        Task<List<string>> GetGenericSynonyms(string callingHost, string category = "vehicles");
    }
}