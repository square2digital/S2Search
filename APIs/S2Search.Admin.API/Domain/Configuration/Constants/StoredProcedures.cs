using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Configuration.Constants
{
    public static class StoredProcedures
    {
        public const string GetSearchIndexQueryCredentials = "[ClientConfigurationApi].[GetSearchIndexQueryCredentialsByCustomerEndpoint]";
        public const string GetTheme = "[ClientConfigurationApi].[GetThemeByCustomerEndpoint]";
        public const string GetConfigurationForSearchIndex = "[ClientConfigurationApi].[GetConfigurationForSearchIndex]";
        public const string GetGenericSynonymsByCategory = "[ClientConfigurationApi].[GetGenericSynonymsByCategory]";
    }
}
