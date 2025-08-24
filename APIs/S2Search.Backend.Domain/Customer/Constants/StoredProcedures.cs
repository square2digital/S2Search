namespace S2Search.Backend.Domain.Customer.Constants;

public static class StoredProcedures
{
    public const string AddSearchIndex = "[CustomerResourceApi].[AddSearchIndex]";
    public const string GetSearchIndex = "[CustomerResourceApi].[GetSearchIndex]";
    public const string GetSearchIndexFull = "[CustomerResourceApi].[GetSearchIndexFull]";
    public const string GetSearchIndexKeysForCustomer = "[CustomerResourceApi].[GetSearchIndexKeysForCustomer]";
    public const string GetSearchIndexByFriendlyName = "[CustomerResourceApi].[GetSearchIndexByFriendlyName]";
    public const string GetActiveCustomerPricingTiers = "[CustomerResourceApi].[GetActiveCustomerPricingTiers]";
    public const string GetSearchIndexQueryCredentials = "[CustomerResourceApi].[GetSearchIndexQueryCredentialsByCustomerEndpoint]";

    public const string AddFeed = "[CustomerResourceApi].[AddFeed]";
    public const string GetLatestFeed = "[CustomerResourceApi].[GetLatestFeed]";

    public const string GetFeedCredentials = "[SFTPGoServicesFunc].[GetFeedCredentials]";
    public const string GetFeedCredentialsUsername = "[CustomerResourceApi].[GetFeedCredentialsUsername]";

    public const string AddNotificationRule = "[CustomerResourceApi].[AddNotificationRule]";
    public const string SupersedeNotificationRule = "[CustomerResourceApi].[SupersedeNotificationRule]";
    public const string GetNotificationRules = "[CustomerResourceApi].[GetNotificationRules]";
    public const string GetNotificationRuleById = "[CustomerResourceApi].[GetNotificationRuleById]";

    public const string AddNotification = "[CustomerResourceApi].[AddNotification]";
    public const string GetNotificationsForSearchIndex = "[CustomerResourceApi].[GetNotificationsForSearchIndex]";

    public const string AddSearchInterface = "[CustomerResourceApi].[AddSearchInterface]";
    public const string GetLatestSearchInterface = "[CustomerResourceApi].[GetLatestSearchInterface]";

    public const string AddSynonym = "[CustomerResourceApi].[AddSynonym]";
    public const string GetSynonymById = "[CustomerResourceApi].[GetSynonymById]";
    public const string GetSynonymByKeyWord = "[CustomerResourceApi].[GetSynonymByKeyWord]";
    public const string GetSynonyms = "[CustomerResourceApi].[GetSynonyms]";
    public const string SupersedeSynonym = "[CustomerResourceApi].[SupersedeSynonym]";
    public const string UpdateSynonym = "[CustomerResourceApi].[UpdateSynonym]";

    public const string GetCustomerById = "[CustomerResourceApi].[GetCustomerByID]";
    public const string GetCustomerFull = "[CustomerResourceApi].[GetCustomerFull]";

    public const string GetThemeById = "[CustomerResourceApi].[GetThemeById]";
    public const string GetThemeByCustomerId = "[CustomerResourceApi].[GetThemeByCustomerId]";
    public const string GetThemeBySearchIndexId = "[CustomerResourceApi].[GetThemeBySearchIndexId]";
    public const string UpdateTheme = "[CustomerResourceApi].[UpdateTheme]";

    public const string GetDashboardSummaryForCustomer = "[CustomerResourceApi].[GetDashboardSummaryForCustomer]";

    public const string GetConfigurationForSearchIndex = "[ClientConfigurationApi].[GetConfigurationForSearchIndex]";
    public const string InsertOrUpdateSearchConfigurationValueById = "[CustomerResourceApi].[InsertOrUpdateSearchConfigurationValueById]";

    public const string GetSearchInsightsByDataCategories = "[CustomerResourceApi].[GetSearchInsightsByDataCategories]";
    public const string GetSearchInsightsSearchCountByDateRange = "[CustomerResourceApi].[GetSearchInsightsSearchCountByDateRange]";

    public const string GetTheme = "[ClientConfigurationApi].[GetThemeByCustomerEndpoint]";
    public const string GetGenericSynonymsByCategory = "[ClientConfigurationApi].[GetGenericSynonymsByCategory]";
}