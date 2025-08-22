namespace Services.Interfaces.Managers
{
    public interface ICacheManager
    {
        Task DeleteKeysByWildcard(string key);
    }
}
