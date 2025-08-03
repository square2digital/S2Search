namespace Services.Helpers
{
    public static class StringHelpers
    {
        public static string FormatCallingHost(string callingHost)
        {
            if (callingHost.Contains("http://"))
            {
                callingHost = callingHost.Replace("http://", string.Empty);
            }

            if (callingHost.Contains("https://"))
            {
                callingHost = callingHost.Replace("https://", string.Empty);
            }

            if (callingHost.Contains("www."))
            {
                callingHost = callingHost.Replace("www.", string.Empty);
            }

            int lastSlash = callingHost.LastIndexOf('/');
            callingHost = (lastSlash > -1) ? callingHost.Substring(0, lastSlash) : callingHost;

            return callingHost;
        }
    }
}