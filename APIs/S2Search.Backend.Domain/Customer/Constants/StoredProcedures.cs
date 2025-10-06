namespace S2Search.Backend.Domain.Customer.Constants;

public static class StoredProcedures
{
    // ***********
    // Admin
    // ***********
    public const string AddFeed = "add_feed";
    public const string AddSearchIndex = "add_search_index";
    public const string AddSearchInterface = "add_search_interface";
    public const string AddSynonym = "add_synonym";
    public const string GetCustomerById = "get_customer_by_id";
    public const string GetCustomerFull = "get_customer_full";
    public const string GetLatestFeed = "get_latest_feed";
    public const string GetLatestSearchInterface = "get_latest_search_interface";
    public const string GetSearchIndex = "get_search_index";
    public const string GetSearchIndexByFriendlyName = "get_search_index_by_friendly_name";
    public const string GetSearchIndexFull = "get_search_index_full";
    public const string GetSearchIndexKeysForCustomer = "get_search_index_keys_for_customer";
    public const string GetSearchInsightsByDataCategories = "get_search_insights_by_data_categories";
    public const string GetSearchInsightsSearchCountByDateRange = "get_search_insights_search_count_by_date_range";
    public const string GetSynonymById = "get_synonym_by_id";
    public const string GetSynonymByKeyWord = "get_synonym_by_key_word";
    public const string GetSynonyms = "get_synonyms";
    public const string GetThemeByCustomerId = "get_theme_by_customer_id";
    public const string GetThemeById = "get_theme_by_id";
    public const string GetThemeBySearchIndexId = "get_theme_by_search_index_id";
    public const string InsertOrUpdateSearchConfigurationValueById = "insert_or_update_search_configuration_value_by_id";
    public const string SupersedeNotificationRule = "supersede_notification_rule";
    public const string SupersedeSynonym = "supersede_synonym";
    public const string UpdateSynonym = "update_synonym";
    public const string UpdateTheme = "update_theme";

    // ***********
    // Configuration
    // ***********
    public const string GetConfigurationForSearchIndex = "get_configuration_for_search_index";
    public const string GetGenericSynonymsByCategory = "get_generic_synonyms_by_category";
    public const string GetTheme = "get_theme_by_customer_endpoint";
    public const string GetSearchIndexQueryCredentials = "get_search_index_query_credentials_by_customer_endpoint";

    // ***********
    // Feed
    // ***********
    public const string GetFeedCredentialsUsername = "get_feed_credentials_username";

    // ***********
    // SFTPGoServicesFunc
    // ***********
    public const string GetFeedCredentials = "get_feed_credentials";
}