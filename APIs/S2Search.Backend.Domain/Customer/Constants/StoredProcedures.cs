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


    public const string AddSearchIndex = "add_search_index"; // DONE
    //public const string AddSearchInterface = "add_search_interface";
    public const string AddSynonym = "add_synonym"; // DONE

    //public const string GetLatestSearchInterface = "get_latest_search_interface";

    //public const string GetSearchIndexKeysForCustomer = "get_search_index_keys_for_customer";
    public const string GetSearchInsightsByDataCategories = "get_search_insights_by_data_categories"; // DONE
    public const string GetSearchInsightsSearchCountByDateRange = "get_search_insights_search_count_by_date_range"; // DONE

    public const string GetSynonyms = "get_synonyms"; // DONE
    public const string GetThemeByCustomerId = "get_theme_by_customer_id"; // DONE
    public const string GetThemeById = "get_theme_by_id"; // DONE
    public const string GetThemeBySearchIndexId = "get_theme_by_search_index_id"; // DONE
    //public const string InsertOrUpdateSearchConfigurationValueById = "insert_or_update_search_configuration_value_by_id";
    //public const string SupersedeNotificationRule = "supersede_notification_rule";

    public const string UpdateTheme = "update_theme"; // DONE

    // ***********
    // Configuration
    // ***********
    //public const string GetConfigurationForSearchIndex = "get_configuration_for_search_index";
    //public const string GetGenericSynonymsByCategory = "get_generic_synonyms_by_category";
    public const string GetTheme = "get_theme_by_customer_endpoint"; // DONE
    public const string GetSearchIndexQueryCredentials = "get_search_index_query_credentials_by_customer_endpoint"; // DONE

    // ***********
    // Feed
    // ***********
    public const string GetFeedCredentialsUsername = "get_feed_credentials_username"; // DONE

    // ***********
    // SFTPGoServicesFunc
    // ***********
    public const string GetFeedCredentials = "get_feed_credentials"; // DONE

    // ***********
    // Functions
    // ***********
    public const string GetFeedDataFormat = "get_feed_data_format";
    public const string GetCurrentFeedDocuments = "get_current_feed_documents";
    public const string GetCurrentFeedDocumentsTotal = "get_current_feed_documents_total";
    public const string MergeFeedDocuments = "merge_feed_documents";

    public const string GetSearchIndexCredentials = "get_search_index_credentials";
    public const string GetSearchIndexFeedProcessingData = "get_search_index_feed_processing_data";

    public const string GetLatestGenericSynonyms = "get_latest_generic_synonyms_by_category";

    public const string AddDataPoints = "add_data_points";
    public const string AddSearchRequest = "add_search_request";
}