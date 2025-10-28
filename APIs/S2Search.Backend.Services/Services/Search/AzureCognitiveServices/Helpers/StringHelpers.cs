namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers
{
    public static class StringHelpers
    {
        public static string FormatCustomerEndpoint(string customerEndpoint)
        {
            if (customerEndpoint.Contains("http://"))
            {
                customerEndpoint = customerEndpoint.Replace("http://", string.Empty);
            }

            if (customerEndpoint.Contains("https://"))
            {
                customerEndpoint = customerEndpoint.Replace("https://", string.Empty);
            }

            if (customerEndpoint.Contains("www."))
            {
                customerEndpoint = customerEndpoint.Replace("www.", string.Empty);
            }

            int lastSlash = customerEndpoint.LastIndexOf('/');
            customerEndpoint = lastSlash > -1 ? customerEndpoint.Substring(0, lastSlash) : customerEndpoint;

            return customerEndpoint;
        }
    }
}