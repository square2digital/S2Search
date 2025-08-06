using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces
{
    public interface ISearchIndexQueryCredentialsProvider
    {
        Task<SearchIndexQueryCredentials> GetAsync(string callingHost);
    }
}