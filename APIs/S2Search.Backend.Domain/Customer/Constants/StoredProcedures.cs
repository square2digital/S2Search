namespace S2Search.Backend.Domain.Customer.Constants;

public static class StoredProcedures
{
    // ***********
    // Admin
    // ***********
    public const string AddFeed = "add_feed";
    public const string AddSearchIndex = "AddSearchIndex";
    public const string AddSearchInterface = "AddSearchInterface";
    public const string AddSynonym = "AddSynonym";
    public const string GetCustomerById = "GetCustomerByID";
    public const string GetCustomerFull = "GetCustomerFull";
    public const string GetLatestFeed = "GetLatestFeed";
    public const string GetLatestSearchInterface = "GetLatestSearchInterface";
    public const string GetSearchIndex = "GetSearchIndex";
    public const string GetSearchIndexByFriendlyName = "GetSearchIndexByFriendlyName";
    public const string GetSearchIndexFull = "GetSearchIndexFull";
    public const string GetSearchIndexKeysForCustomer = "GetSearchIndexKeysForCustomer";
    public const string GetSearchInsightsByDataCategories = "GetSearchInsightsByDataCategories";
    public const string GetSearchInsightsSearchCountByDateRange = "GetSearchInsightsSearchCountByDateRange";
    public const string GetSynonymById = "GetSynonymById";
    public const string GetSynonymByKeyWord = "GetSynonymByKeyWord";
    public const string GetSynonyms = "GetSynonyms";
    public const string GetThemeByCustomerId = "GetThemeByCustomerId";
    public const string GetThemeById = "GetThemeById";
    public const string GetThemeBySearchIndexId = "GetThemeBySearchIndexId";
    public const string InsertOrUpdateSearchConfigurationValueById = "InsertOrUpdateSearchConfigurationValueById";
    public const string SupersedeNotificationRule = "SupersedeNotificationRule";
    public const string SupersedeSynonym = "SupersedeSynonym";
    public const string UpdateSynonym = "UpdateSynonym";
    public const string UpdateTheme = "UpdateTheme";

    // ***********
    // Configuration
    // ***********
    public const string GetConfigurationForSearchIndex = "GetConfigurationForSearchIndex";
    public const string GetGenericSynonymsByCategory = "GetGenericSynonymsByCategory";
    public const string GetTheme = "GetThemeByCustomerEndpoint";
    public const string GetSearchIndexQueryCredentials = "GetSearchIndexQueryCredentialsByCustomerEndpoint";

    // ***********
    // Feed
    // ***********
    public const string GetFeedCredentialsUsername = "GetFeedCredentialsUsername";

    // ***********
    // SFTPGoServicesFunc
    // ***********
    public const string GetFeedCredentials = "GetFeedCredentials";
}