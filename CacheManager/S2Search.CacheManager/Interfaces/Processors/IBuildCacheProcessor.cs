namespace S2Search.CacheManager.Interfaces.Processors
{
    internal interface IBuildCacheProcessor
    {
        Task ProcessAsync(string customerEndpoint);
    }
}