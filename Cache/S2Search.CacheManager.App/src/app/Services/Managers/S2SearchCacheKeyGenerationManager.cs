namespace Services.Managers
{
    public static class S2SearchCacheKeyGenerationManager
    {
        public static string Generate(string host)
        {
            return $"{host}:";
        }
    }
}
