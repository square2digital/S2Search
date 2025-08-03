using System.Collections.Generic;

namespace Services.Helpers
{
    public static class ApiManagerHelper
    {
        public static Dictionary<string, List<string>> GetHeader(string subscriptionName, string subscriptionKey)
        {
            var headers = new Dictionary<string, List<string>>();
            headers.Add(subscriptionName, new List<string>() { subscriptionKey });

            return headers;
        }
    }
}