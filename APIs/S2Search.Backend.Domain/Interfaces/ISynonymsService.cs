namespace S2Search.Backend.Domain.Interfaces;

public interface ISynonymsService
{
    Task<List<string>> GetGenericSynonyms(string customerEndpoint, string category = "vehicles");
}