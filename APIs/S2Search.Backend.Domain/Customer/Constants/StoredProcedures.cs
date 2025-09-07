namespace S2Search.Backend.Domain.Customer.Constants;

public static class StoredProcedures
{
    // ***********
    // Admin
    // ***********
    public const string AddFeed = "[Admin].[AddFeed]";
    public const string AddSearchIndex = "[Admin].[AddSearchIndex]";
    public const string AddSearchInterface = "[Admin].[AddSearchInterface]";
    public const string AddSynonym = "[Admin].[AddSynonym]";
    public const string GetCustomerById = "[Admin].[GetCustomerByID]";
    public const string GetCustomerFull = "[Admin].[GetCustomerFull]";
    public const string GetFeedCredentialsUsername = "[Admin].[GetFeedCredentialsUsername]";
    public const string GetSearchIndexQueryCredentials = "[CustomerResourceApi].[GetSearchIndexQueryCredentialsByCustomerEndpoint]";
    public const string GetLatestFeed = "[Admin].[GetLatestFeed]";
    public const string GetLatestSearchInterface = "[Admin].[GetLatestSearchInterface]";
    public const string GetSearchIndex = "[Admin].[GetSearchIndex]";
    public const string GetSearchIndexByFriendlyName = "[Admin].[GetSearchIndexByFriendlyName]";
    public const string GetSearchIndexFull = "[Admin].[GetSearchIndexFull]";
    public const string GetSearchIndexKeysForCustomer = "[Admin].[GetSearchIndexKeysForCustomer]";
    public const string GetSearchInsightsByDataCategories = "[Admin].[GetSearchInsightsByDataCategories]";
    public const string GetSearchInsightsSearchCountByDateRange = "[Admin].[GetSearchInsightsSearchCountByDateRange]";
    public const string GetSynonymById = "[Admin].[GetSynonymById]";
    public const string GetSynonymByKeyWord = "[Admin].[GetSynonymByKeyWord]";
    public const string GetSynonyms = "[Admin].[GetSynonyms]";
    public const string GetThemeByCustomerId = "[Admin].[GetThemeByCustomerId]";
    public const string GetThemeById = "[Admin].[GetThemeById]";
    public const string GetThemeBySearchIndexId = "[Admin].[GetThemeBySearchIndexId]";
    public const string InsertOrUpdateSearchConfigurationValueById = "[Admin].[InsertOrUpdateSearchConfigurationValueById]";
    public const string SupersedeNotificationRule = "[Admin].[SupersedeNotificationRule]";
    public const string SupersedeSynonym = "[Admin].[SupersedeSynonym]";
    public const string UpdateSynonym = "[Admin].[UpdateSynonym]";
    public const string UpdateTheme = "[Admin].[UpdateTheme]";

    // ***********
    // Configuration
    // ***********
    public const string GetConfigurationForSearchIndex = "[Configuration].[GetConfigurationForSearchIndex]";
    public const string GetGenericSynonymsByCategory = "[Configuration].[GetGenericSynonymsByCategory]";
    public const string GetTheme = "[Configuration].[GetThemeByCustomerEndpoint]";

    // ***********
    // SFTPGoServicesFunc
    // ***********
    public const string GetFeedCredentials = "[SFTPGoServicesFunc].[GetFeedCredentials]";
}