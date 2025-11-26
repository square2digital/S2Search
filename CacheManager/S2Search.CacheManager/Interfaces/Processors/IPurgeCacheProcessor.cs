namespace S2Search.CacheManager.Interfaces.Processors
{
    public interface IPurgeCacheProcessor
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}
