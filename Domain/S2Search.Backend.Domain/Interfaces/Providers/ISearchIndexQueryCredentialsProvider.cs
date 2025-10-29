using S2Search.Backend.Domain.Configuration.SearchResources.Credentials;

namespace Services.Interfaces;

public interface ISearchIndexQueryCredentialsProvider
{
    Task<SearchIndexQueryCredentials> GetAsync(string customerEndpoint);
}