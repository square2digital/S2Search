namespace Services.Managers
{
    public static class S2SearchCacheKeyGenerationManager
    {
        public static string Generate(string host)
        {
            var formattedHost = host.Replace(":", string.Empty);
            return $"{formattedHost}:";
        }
    }
}
