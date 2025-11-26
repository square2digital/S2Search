namespace S2Search.CacheManager.Interfaces.Managers
{
    public interface ICacheManager
    {
        Task DeleteKeysByWildcard(string key);
    }
}
