namespace S2Search.Backend.Domain.Customer.Constants;

public static class StoredProcedures
{

    //delete_feed_credentials - ISNT REFERENCED
    //supersede_latest_feed - ISNT REFERENCED
    //update_feed_credentials - ISNT REFERENCED

    // ***********
    // Admin
    // ***********
    public const string AddFeed = "add_feed"; // OK
    public const string GetCustomerById = "get_customer_by_id"; // OK
    public const string GetCustomerFull = "get_customer_full"; // OK
    public const string GetLatestFeed = "get_latest_feed"; // OK
    public const string GetSearchIndex = "get_search_index"; // OK
    public const string GetSearchIndexByFriendlyName = "get_search_index_by_friendly_name"; // OK
    public const string GetSearchIndexFull = "get_search_index_full"; // OK
    public const string GetSynonymById = "get_synonym_by_id"; // OK
    public const string GetSynonymByKeyWord = "get_synonym_by_key_word"; // OK
    public const string SupersedeSynonym = "supersede_synonym"; // OK
    public const string UpdateSynonym = "update_synonym"; // OK


    public const string AddSearchIndex = "add_search_index"; // OK
    public const string AddSynonym = "add_synonym"; // OK

    public const string GetSearchInsightsByDataCategories = "get_search_insights_by_data_categories"; // OK
    public const string GetSearchInsightsSearchCountByDateRange = "get_search_insights_search_count_by_date_range"; // OK

    public const string GetSynonyms = "get_synonyms"; // OK
    public const string GetThemeByCustomerId = "get_theme_by_customer_id"; // OK
    public const string GetThemeById = "get_theme_by_id"; // OK
    public const string GetThemeBySearchIndexId = "get_theme_by_search_index_id"; // OK

    public const string UpdateTheme = "update_theme"; // OK

    // ***********
    // Configuration
    // ***********
    public const string GetTheme = "get_theme_by_customer_endpoint"; // OK
    public const string GetSearchIndexQueryCredentials = "get_search_index_query_credentials_by_customer_endpoint"; // OK

    // ***********
    // Feed
    // ***********
    public const string GetFeedCredentialsUsername = "get_feed_credentials_username"; // OK

    // ***********
    // SFTPGoServicesFunc
    // ***********
    public const string GetFeedCredentials = "get_feed_credentials"; // OK

    // ***********
    // Functions
    // ***********
    public const string GetFeedDataFormat = "get_feed_data_format";  // OK
    public const string GetCurrentFeedDocuments = "get_current_feed_documents";
    public const string GetCurrentFeedDocumentsTotal = "get_current_feed_documents_total";
    public const string MergeFeedDocuments = "merge_feed_documents"; // OK

    public const string GetSearchIndexCredentials = "get_search_index_credentials"; // OK
    public const string GetSearchIndexFeedProcessingData = "get_search_index_feed_processing_data"; // OK

    public const string GetLatestGenericSynonyms = "get_latest_generic_synonyms_by_category"; // OK

    public const string AddDataPoints = "add_data_points"; // OK
    public const string AddSearchRequest = "add_search_request"; // OK
}