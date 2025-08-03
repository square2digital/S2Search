using Nest;

namespace Domain.Interfaces
{
    public interface IElasticSearchClientProvider
    {
        ElasticClient GetElasticClient();
    }
}