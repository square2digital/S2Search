DO $$
DECLARE
    --SearchIndex variables
    CustomerId UUID := 'afeb217b-813a-4b9c-82ec-e0221d5e95b1';
    SearchInstanceId UUID := '97032266-c1c0-4278-8816-053bbc3a1036';
    ResourceGroup   TEXT := 'S2Search';
    SearchInstanceEndpoint   TEXT := 'https://s2-search-dev.search.windows.net';
    StorageURI   TEXT := 'https://s2storagedev.blob.core.windows.net';
    AdminKey  TEXT  := 'meh33Ur6Zd7oGUv201TvZAXD5mqTOH9QN1YtFePp86AzSeDwh11h';
    SecondaryKey   TEXT := 'Jc6htHDfDNK6MAgGm80ZBFqjsb0RZjkdwfSYxjMh6XAzSeAHeUua';
    QueryKey TEXT := 'JTli3f7UNsq6UP5aarwr6kEuXLziImNklC1EZlI3zSAzSeDZXlvC';
    Replicas INT := 1;
    Partitions INT := 1;
    IsShared BOOLEAN := TRUE;
    SearchIndexName TEXT := 'vehicles-' || LEFT(gen_random_UUID()::text, 8);
    --Feeds
    FeedType TEXT := 'FTPS';
    FeedCron TEXT := '5 * * * *';
    DataFormat TEXT := 'DMS14';
    
    --SearchInterfaces
    InterfaceType TEXT := 'API_Consumption';
    InterfaceLogoURL TEXT := NULL;
    InterfaceBannerStyle TEXT := NULL;
    
    --Synonyms
    SynonymId_1 UUID := '36baeafb-a843-4eb1-a2a9-4457d2eabe0d';
    KeyWord_1 TEXT := 'BMW';
    SolrFormat_1 TEXT := 'beema, bimma => BMW';
    
    SynonymId_2 UUID := '08719d6f-4e7f-4c3b-9b9b-b83100441012';
    KeyWord_2 TEXT := 'volkswagen';
    SolrFormat_2 TEXT := 'VW => Volkswagen';
    
    --SearchInstance Keys
    SearchInstanceKeyId_AdminKey UUID := '317e8b8a-c274-499c-a4fb-bacdf8d25e81';
    SearchInstanceKeyId_SecondaryAdminKey UUID := '3e0217e5-38be-4616-a81d-c1693c83aa5c';
    SearchInstanceKeyId_QueryKey UUID := 'fea6648a-f781-4ec0-b8f6-ecf26eb820ac';
    
    -- **************
    -- Configuration
    -- **************
    
    -- Configurations Option 1 - Enable Auto Complete
    SearchConfiguration_EnableAutoComplete_Id UUID := '4d833bb7-c52b-49ed-b42d-84a9814b9274';
    SearchConfiguration_EnableAutoComplete_Key TEXT := 'EnableAutoComplete';
    SearchConfiguration_EnableAutoComplete_FriendlyName TEXT := 'Enable Auto Complete';
    SearchConfiguration_EnableAutoComplete_Description TEXT := 'if true, this will configure the search bar to include auto complete suggestions based on the users text inputs';
    
    -- Configurations Option 2 - Hide Icon Vehicle Counts
    SearchConfiguration_HideIconVehicleCounts_Id UUID := 'd4d0978e-980c-4237-be79-f2e0ffc0ae06';
    SearchConfiguration_HideIconVehicleCounts_Key TEXT := 'HideIconVehicleCounts';
    SearchConfiguration_HideIconVehicleCounts_FriendlyName TEXT := 'Hide Icon Vehicle Counts';
    SearchConfiguration_HideIconVehicleCounts_Description TEXT := 'if true, this will hide the icon vehicle counts on the top nav bar';
    
    -- Configurations Option 3 - Placeholder Text Array
    SearchConfiguration_PlaceholderText_Key_1 TEXT := 'PlaceholderText_1';
    SearchConfiguration_PlaceholderText_Key_2 TEXT := 'PlaceholderText_2';
    SearchConfiguration_PlaceholderText_Key_3 TEXT := 'PlaceholderText_3';
    SearchConfiguration_PlaceholderText_Key_4 TEXT := 'PlaceholderText_4';
    SearchConfiguration_PlaceholderText_Key_5 TEXT := 'PlaceholderText_5';
    
    Separator TEXT := '...     ';
    SearchConfiguration_PlaceholderText_Value_1 TEXT := CONCAT('Mercedes Benz S63 AMG', Separator);
    SearchConfiguration_PlaceholderText_Value_2 TEXT := CONCAT('Porsche Green GT3RS', Separator);
    SearchConfiguration_PlaceholderText_Value_3 TEXT := CONCAT('BMW 7 series Silver', Separator);
    SearchConfiguration_PlaceholderText_Value_4 TEXT := CONCAT('Lexus LS460 Black', Separator);
    SearchConfiguration_PlaceholderText_Value_5 TEXT := CONCAT('Honda Type R Red', Separator);
    
    -- ***********
    -- customers
    -- ***********
    -- Azure B2C Test User ID Guid -> 37a0eb6c-fd38-4b11-9486-e61ed6745953
    
    -- Test User -> Jonathan Gilmartin - user of S2 Demo
    Azure_B2C_User_Jonathan_Gilmartin UUID := 'a870f617-c469-4fdc-b76d-98a990577583';
    CustomerId_JGilmartin_Motors UUID := Azure_B2C_User_Jonathan_Gilmartin;
    
    -- ********************************************
    -- ** S2-Demo Endpoints
    -- ********************************************
    S2DemoEndpoint TEXT := 'demo.s2search.co.uk';
    -- BLUE - Corporate colours for Square 2 Digital
    
    -- endpoint overrides
    LocalDevEndpoint TEXT := 'localhost:2997';
    LocalK8sEndpoint TEXT := 'localhost:3000';
    
    -- ********************************************
    -- to override endpoints - useful to setting a search instance to another URL or to localhost
    -- ********************************************
    
    -- ************************
    -- S2 Demo  - customer 1
    -- ************************
    BusinessName_1 TEXT := 'S2 Demo';
    CustomerIndexName_1 TEXT := 's2-demo-vehicles';
    SearchIndexId_1 UUID := '8c663063-4217-4f54-973f-8faec6131b5b';
    ThemeId_1 UUID := 'f3a9c2e7-4b6e-4d9a-8f3e-9c1d2a7b5e6f';
    ThemeLogoURL_1 TEXT := StorageURI || '/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg';
    ThemeMissingImageURL_1 TEXT := StorageURI || '/assets/image-coming-soon.jpg';
    ThemePrimaryThemeColour_1 TEXT := '#006bd1';
    ThemeSecondaryThemeColour_1 TEXT := '#003c75';
    ThemeNavBarColourColour_1 TEXT := '#006bd1';
    
    /***************************************************************************************
    Customer Pricing Tiers
    ***************************************************************************************/
    SkuIdFree TEXT := 'FREE';
    
    /***************************************************************************************
    Feed Credentials
    ***************************************************************************************/
    -- SearchIndexId is set as S2 Demo
    FeedSearchIndexId_1 UUID := SearchIndexId_1;
    FeedId_1 UUID := '7bcc246b-c8e0-4d91-bb8a-ec5f8c7b3230';
    FeedUsername_1 TEXT := 's2demo_FTP_1';
    FeedPasswordHash_1 TEXT := 'xh6NPbvqmDhH6E2vK3mJ';
    
    /***************************************************************************************
    Search Instance
    ***************************************************************************************/
    SearchInstanceName TEXT := 's2-search-dev';
    SubscriptionId UUID := 'f8cff945-b5e5-462a-9786-d69bd7a0eb34';
    ServiceLocation TEXT := 'West Europe';
    AzurePricingTier TEXT := 'Standard';
    
    /***************************************************************************************
    Table Reset
    ----------------------------------------------------------------------------------------
    Truncate all tables to start with a fresh setup
    ***************************************************************************************/

BEGIN
    -- override endpoint for demo
    S2DemoEndpoint := LocalDevEndpoint;

    -- your logic here
    
    DELETE FROM customers;
    DELETE FROM feed_credentials;
    DELETE FROM feed_current_documents;
    DELETE FROM feeds;
    DELETE FROM search_configuration;
    DELETE FROM search_index;
    DELETE FROM search_index_request_log;
    DELETE FROM search_insights_data;
    DELETE FROM search_instance_keys;
    DELETE FROM search_instances;
    DELETE FROM synonyms;
    DELETE FROM themes;
    
    /***************************************************************************************
    Uncomment this if you want to clear down the insights
    ***************************************************************************************/
    TRUNCATE TABLE search_insights_data;
    TRUNCATE TABLE search_index_request_log;
    
    RAISE NOTICE '********************************';
    RAISE NOTICE 'Inserting Search Instances Entry';
    RAISE NOTICE '********************************';

    INSERT INTO search_instances
    (
		id,
        customer_id,
		service_name,
		location,
		pricing_tier,
		replicas,
		partitions,
		is_shared,
		type,
		root_endpoint
    )
    VALUES
    (
		SearchInstanceId, -- or your actual Id (GUID)
        CustomerId,
		SearchInstanceName, -- ServiceName
		ServiceLocation, -- Location
		AzurePricingTier, -- PricingTier
		Replicas, -- Replicas
		Partitions, -- Partitions
		IsShared, -- IsShared (0 for false, 1 for true)
		'Production', -- Type
		SearchInstanceEndpoint  -- RootEndpoint
    );
    
    
    RAISE NOTICE '********************************';
    RAISE NOTICE 'Inserting Search Resource Keys';
    RAISE NOTICE '********************************';
    
    INSERT INTO search_instance_keys
    (
		id,
		search_instance_id,
		key_type,
		name,
		api_key,
		created_date,
		modified_date,
		is_latest
    )
    VALUES
    (
		SearchInstanceKeyId_AdminKey,
		SearchInstanceId,
		'Admin',
		'Primary Admin key',
		AdminKey,
		CURRENT_TIMESTAMP,
		null,
		TRUE
    );

    INSERT INTO search_instance_keys
    (
		id,
		search_instance_id,
		key_type,
		name,
		api_key,
		created_date,
		modified_date,
		is_latest
    )
    VALUES
    (
		SearchInstanceKeyId_SecondaryAdminKey,
		SearchInstanceId,
		'Admin',
		'Secondary Admin key',
		SecondaryKey,
		CURRENT_TIMESTAMP,
		null,
        TRUE
    );

    INSERT INTO search_instance_keys
    (
		id,
		search_instance_id,
		key_type,
		name,
		api_key,
		created_date,
		modified_date,
		is_latest
    )
    VALUES
    (
		SearchInstanceKeyId_QueryKey,
		SearchInstanceId,
		'Query',
		'Query key',
		QueryKey,
		CURRENT_TIMESTAMP,
		null,
		TRUE
    );
        
    RAISE NOTICE '************************';
    RAISE NOTICE 'Inserting Test Customers';
    RAISE NOTICE '************************';
    
    RAISE NOTICE '************************';
    RAISE NOTICE 'Inserting Test Customer 1';
    RAISE NOTICE '************************';
    
    INSERT INTO customers
    (
		id,
		business_name,
        customer_endpoint,
		created_date,
		modified_date
    )
    VALUES
    (
		CustomerId_JGilmartin_Motors,
		BusinessName_1,
        S2DemoEndpoint,
		CURRENT_TIMESTAMP,
		NULL
    );
    
    RAISE NOTICE '********************************';
    RAISE NOTICE 'Inserting Search Index for Test Customer 1';
    RAISE NOTICE '********************************';
    
    INSERT INTO search_index
    (
		id,
		search_instance_id,
		index_name,
		friendly_name,
		customer_id,
		created_date,
		pricing_sku_id
    )
    VALUES
    (
		SearchIndexId_1,
		SearchInstanceId,
		CustomerIndexName_1,
		BusinessName_1,
		CustomerId_JGilmartin_Motors,
		CURRENT_TIMESTAMP,
		SkuIdFree
    );
    
    RAISE NOTICE '********************************';
    RAISE NOTICE 'Inserting Feed Entry for Test Customer 1';
    RAISE NOTICE '********************************';
    
    INSERT INTO feeds
    (
        id,
		feed_type,
		feed_schedule_cron,
		search_index_id,
		data_format,
		created_date,
		superseded_date,
		is_latest
    )
    VALUES
    (
        gen_random_UUID(),
		FeedType,
		FeedCron,
		SearchIndexId_1,
		DataFormat,
		CURRENT_TIMESTAMP,
		null,
		TRUE
    );
    
    RAISE NOTICE '********************************';
    RAISE NOTICE 'Inserting Synonyms';
    RAISE NOTICE '********************************';
    
    INSERT INTO synonyms
    (
		id,
		search_index_id,
		key_word,
		solr_format,
        created_date,
        is_latest
    )
    VALUES
    (
		SynonymId_1,
		SearchIndexId_1,
		KeyWord_1,
		SolrFormat_1,
        CURRENT_TIMESTAMP,
        TRUE
    );
    
    INSERT INTO synonyms
    (
		id,
		search_index_id,
		key_word,
		solr_format,
        created_date,
        is_latest
    )
    VALUES
    (
		SynonymId_2,
		SearchIndexId_1,
		KeyWord_2,
		SolrFormat_2,
        CURRENT_TIMESTAMP,
        TRUE
    );
    
    RAISE NOTICE '********************************';
    RAISE NOTICE 'Customer 1 - S2 Demo - Theme Details';
    RAISE NOTICE '********************************';
    
    INSERT INTO themes
    (
		id,
		primary_hex_colour,
		secondary_hex_colour,
		nav_bar_hex_colour,
		logo_url,
		missing_image_url,
		customer_id,
		search_index_id,
		created_date,
		modified_date
    )
    VALUES
    (
		ThemeId_1,
		ThemePrimaryThemeColour_1,
		ThemeSecondaryThemeColour_1,
		ThemeNavBarColourColour_1,
		ThemeLogoURL_1,
		ThemeMissingImageURL_1,
		CustomerId_JGilmartin_Motors,
		SearchIndexId_1,
		CURRENT_TIMESTAMP,
		NULL
    );
      
    RAISE NOTICE '*******************************************';
    RAISE NOTICE 'Search Index 1 FeedCredentials';
    RAISE NOTICE '*******************************************';
    
    INSERT INTO feed_credentials
	(
        id,
		search_index_id,
		username,
		password_hash,
		created_date,
		modified_date
    )
    VALUES
	(
		 FeedId_1,
		 FeedSearchIndexId_1,
		 FeedUsername_1,
		 FeedPasswordHash_1,
		 CURRENT_TIMESTAMP,
		 null
	);
    
    RAISE NOTICE '****************************';
    RAISE NOTICE 'Setup S2 Demo Data Configuration Mappings';
    RAISE NOTICE '****************************';
    
    -- Option 1: Enable Auto Complete
    INSERT INTO search_configuration
	(
		id,
		value,
		search_index_id,
		key,
		friendly_name,
		description,
		data_type,
		order_index,
		created_date,
		modified_date
	)
    VALUES
    (
		SearchConfiguration_EnableAutoComplete_Id,
		'true',
		gen_random_UUID(), -- Replace with actual SearchIndexId if needed
		SearchConfiguration_EnableAutoComplete_Key,
		SearchConfiguration_EnableAutoComplete_FriendlyName,
		SearchConfiguration_EnableAutoComplete_Description,
		'Boolean',
		NULL,
		CURRENT_TIMESTAMP,
		NULL
    );
    
    -- Option 2: Hide Icon Vehicle Counts
    INSERT INTO search_configuration
    (
		id,
		value,
		search_index_id,
		key,
		friendly_name,
		description,
		data_type,
		order_index,
		created_date,
		modified_date
    )
    VALUES
    (
		SearchConfiguration_HideIconVehicleCounts_Id,
		'true',
		gen_random_UUID(), -- Replace with actual SearchIndexId if needed
		SearchConfiguration_HideIconVehicleCounts_Key,
		SearchConfiguration_HideIconVehicleCounts_FriendlyName,
		SearchConfiguration_HideIconVehicleCounts_Description,
		'Boolean',
		NULL,
		CURRENT_TIMESTAMP,
		NULL
    );
    
    -- Option 3: Placeholder Text Array (5 rows)
    INSERT INTO search_configuration
    (
		id,
		value,
		search_index_id,
		key,
		friendly_name,
		description,
		data_type,
		order_index,
		created_date,
		modified_date
    )
    VALUES
    (gen_random_UUID(), SearchConfiguration_PlaceholderText_Value_1, gen_random_UUID(), SearchConfiguration_PlaceholderText_Key_1, 'Placeholder Text 1', 'Placeholder for search bar', 'String', 1, CURRENT_TIMESTAMP, NULL),
    (gen_random_UUID(), SearchConfiguration_PlaceholderText_Value_2, gen_random_UUID(), SearchConfiguration_PlaceholderText_Key_2, 'Placeholder Text 2', 'Placeholder for search bar', 'String', 2, CURRENT_TIMESTAMP, NULL),
    (gen_random_UUID(), SearchConfiguration_PlaceholderText_Value_3, gen_random_UUID(), SearchConfiguration_PlaceholderText_Key_3, 'Placeholder Text 3', 'Placeholder for search bar', 'String', 3, CURRENT_TIMESTAMP, NULL),
    (gen_random_UUID(), SearchConfiguration_PlaceholderText_Value_4, gen_random_UUID(), SearchConfiguration_PlaceholderText_Key_4, 'Placeholder Text 4', 'Placeholder for search bar', 'String', 4, CURRENT_TIMESTAMP, NULL),
    (gen_random_UUID(), SearchConfiguration_PlaceholderText_Value_5, gen_random_UUID(), SearchConfiguration_PlaceholderText_Key_5, 'Placeholder Text 5', 'Placeholder for search bar', 'String', 5, CURRENT_TIMESTAMP, NULL);
        
    RAISE NOTICE '********************************';
    RAISE NOTICE 'Script - Complete';
    RAISE NOTICE '********************************';
END

$$ LANGUAGE plpgsql;