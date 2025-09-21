USE S2_Search
GO

BEGIN
    DECLARE @Now datetime = GETUTCDATE()
    
    --SearchIndex variables
    DECLARE @CustomerId uniqueidentifier = 'afeb217b-813a-4b9c-82ec-e0221d5e95b1'
    DECLARE @SearchInstanceId uniqueidentifier = '97032266-c1c0-4278-8816-053bbc3a1036'
    DECLARE @ResourceGroup varchar(255) = 'S2Search'
    DECLARE @SearchInstanceEndpoint varchar(250) = 'https://s2-search-dev.search.windows.net'
    DECLARE @StorageURI varchar(250) = 'https://s2storagedev.blob.core.windows.net'
    DECLARE @AdminKey varchar(255) = 'meh33Ur6Zd7oGUv201TvZAXD5mqTOH9QN1YtFePp86AzSeDwh11h'
    DECLARE @SecondaryKey varchar(255) = 'Jc6htHDfDNK6MAgGm80ZBFqjsb0RZjkdwfSYxjMh6XAzSeAHeUua'
    DECLARE @QueryKey varchar(255) = 'JTli3f7UNsq6UP5aarwr6kEuXLziImNklC1EZlI3zSAzSeDZXlvC'
    DECLARE @Replicas int = 1
    DECLARE @Partitions int = 1
    DECLARE @IsShared bit = 1
    DECLARE @SearchIndexName varchar(255) = 'vehicles-' + LEFT(CONVERT(varchar(255), NEWID()), 8)
    --Feeds
    DECLARE @FeedType varchar(20) = 'FTPS'
    DECLARE @FeedCron varchar(50) = '5 * * * *'
    DECLARE @DataFormat varchar(50) = 'DMS14'
    
    --SearchInterfaces
    DECLARE @InterfaceType varchar(50) = 'API_Consumption'
    DECLARE @InterfaceLogoURL varchar(255) = NULL
    DECLARE @InterfaceBannerStyle varchar(255) = NULL
    
    --Synonyms
    DECLARE @SynonymId_1 uniqueidentifier = '36baeafb-a843-4eb1-a2a9-4457d2eabe0d'
    DECLARE @KeyWord_1 varchar(50) = 'BMW'
    DECLARE @SolrFormat_1 varchar(250) = 'beema, bimma => BMW'
    
    DECLARE @SynonymId_2 uniqueidentifier = '08719d6f-4e7f-4c3b-9b9b-b83100441012'
    DECLARE @KeyWord_2 varchar(50) = 'volkswagen'
    DECLARE @SolrFormat_2 varchar(250) = 'VW => Volkswagen'
    
    --SearchInstance Keys
    DECLARE @SearchInstanceKeyId_AdminKey uniqueidentifier = '317e8b8a-c274-499c-a4fb-bacdf8d25e81'
    DECLARE @SearchInstanceKeyId_SecondaryAdminKey uniqueidentifier = '3e0217e5-38be-4616-a81d-c1693c83aa5c'
    DECLARE @SearchInstanceKeyId_QueryKey uniqueidentifier = 'fea6648a-f781-4ec0-b8f6-ecf26eb820ac'
    
    -- **************
    -- Configuration
    -- **************
    
    -- Configurations Option 1 - Enable Auto Complete
    DECLARE @SearchConfiguration_EnableAutoComplete_Id uniqueidentifier = '4d833bb7-c52b-49ed-b42d-84a9814b9274'
    DECLARE @SearchConfiguration_EnableAutoComplete_Key varchar(150) = 'EnableAutoComplete'
    DECLARE @SearchConfiguration_EnableAutoComplete_FriendlyName varchar(500) = 'Enable Auto Complete'
    DECLARE @SearchConfiguration_EnableAutoComplete_Description varchar(MAX) = 'if true, this will configure the search bar to include auto complete suggestions based on the users text inputs'
    
    -- Configurations Option 2 - Hide Icon Vehicle Counts
    DECLARE @SearchConfiguration_HideIconVehicleCounts_Id uniqueidentifier = 'd4d0978e-980c-4237-be79-f2e0ffc0ae06'
    DECLARE @SearchConfiguration_HideIconVehicleCounts_Key varchar(150) = 'HideIconVehicleCounts'
    DECLARE @SearchConfiguration_HideIconVehicleCounts_FriendlyName varchar(500) = 'Hide Icon Vehicle Counts'
    DECLARE @SearchConfiguration_HideIconVehicleCounts_Description varchar(MAX) = 'if true, this will hide the icon vehicle counts on the top nav bar'
    
    -- Configurations Option 3 - Placeholder Text Array
    DECLARE @SearchConfiguration_PlaceholderText_Key_1 varchar(150) = 'PlaceholderText_1'
    DECLARE @SearchConfiguration_PlaceholderText_Key_2 varchar(150) = 'PlaceholderText_2'
    DECLARE @SearchConfiguration_PlaceholderText_Key_3 varchar(150) = 'PlaceholderText_3'
    DECLARE @SearchConfiguration_PlaceholderText_Key_4 varchar(150) = 'PlaceholderText_4'
    DECLARE @SearchConfiguration_PlaceholderText_Key_5 varchar(150) = 'PlaceholderText_5'
    
    DECLARE @Separator varchar(50) = '...     '
    DECLARE @SearchConfiguration_PlaceholderText_Value_1 varchar(250) = CONCAT('Mercedes Benz S63 AMG', @Separator)
    DECLARE @SearchConfiguration_PlaceholderText_Value_2 varchar(250) = CONCAT('Porsche Green GT3RS', @Separator)
    DECLARE @SearchConfiguration_PlaceholderText_Value_3 varchar(250) = CONCAT('BMW 7 series Silver', @Separator)
    DECLARE @SearchConfiguration_PlaceholderText_Value_4 varchar(250) = CONCAT('Lexus LS460 Black', @Separator)
    DECLARE @SearchConfiguration_PlaceholderText_Value_5 varchar(250) = CONCAT('Honda Type R Red', @Separator)
    
    -- ***********
    -- customers
    -- ***********
    -- Azure B2C Test User ID Guid -> 37a0eb6c-fd38-4b11-9486-e61ed6745953
    
    -- Test User -> Jonathan Gilmartin - user of S2 Demo
    DECLARE @Azure_B2C_User_Jonathan_Gilmartin uniqueidentifier = 'a870f617-c469-4fdc-b76d-98a990577583'
    DECLARE @CustomerId_JGilmartin_Motors uniqueidentifier = @Azure_B2C_User_Jonathan_Gilmartin
    
    -- ********************************************
    -- ** S2-Demo Endpoints
    -- ********************************************
    DECLARE @S2DemoEndpoint varchar(100) = 'demo.s2search.co.uk'
    -- BLUE - Corporate colours for Square 2 Digital
    
    -- endpoint overrides
    DECLARE @LocalDevEndpoint varchar(100) = 'localhost:2997'
    DECLARE @LocalK8sEndpoint varchar(100) = 'localhost:3000'
    
    -- ********************************************
    -- use this to override endpoints - useful to setting a search instance to another URL or to localhost
    -- ********************************************
    SET @S2DemoEndpoint = @LocalDevEndpoint
    
    -- ************************
    -- S2 Demo  - customer 1
    -- ************************
    DECLARE @BusinessName_1 varchar(100) = 'S2 Demo'
    DECLARE @CustomerIndexName_1 varchar(100) = 's2-demo-vehicles'
    DECLARE @SearchIndexId_1 uniqueidentifier = '8c663063-4217-4f54-973f-8faec6131b5b'
    DECLARE @ThemeId_1 uniqueidentifier = 'f3a9c2e7-4b6e-4d9a-8f3e-9c1d2a7b5e6f'
    DECLARE @ThemeLogoURL_1 varchar(1000) = @StorageURI + '/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg'
    DECLARE @ThemeMissingImageURL_1 varchar(1000) = @StorageURI + '/assets/image-coming-soon.jpg'
    DECLARE @ThemePrimaryThemeColour_1 varchar(10) = '#006bd1'
    DECLARE @ThemeSecondaryThemeColour_1 varchar(10) = '#003c75'
    DECLARE @ThemeNavBarColourColour_1 varchar(10) = '#006bd1'
    
    /***************************************************************************************
    Customer Pricing Tiers
    ***************************************************************************************/
    DECLARE @SkuIdFree varchar(50) = 'FREE'
    
    /***************************************************************************************
    Feed Credentials
    ***************************************************************************************/
    -- SearchIndexId is set as S2 Demo
    DECLARE @FeedSearchIndexId_1 uniqueidentifier = @SearchIndexId_1
    DECLARE @FeedId_1 uniqueidentifier = '7bcc246b-c8e0-4d91-bb8a-ec5f8c7b3230'
    DECLARE @FeedUsername_1 varchar(100) = 's2demo_FTP_1'
    DECLARE @FeedPasswordHash_1 varchar(255) = 'xh6NPbvqmDhH6E2vK3mJ'
    
    /***************************************************************************************
    Search Instance
    ***************************************************************************************/
    DECLARE @SearchInstanceName varchar(255) = 's2-search-dev'
    DECLARE @SubscriptionId uniqueidentifier = 'f8cff945-b5e5-462a-9786-d69bd7a0eb34'
    DECLARE @ServiceLocation varchar(50) = 'West Europe'
    DECLARE @AzurePricingTier varchar(50) = 'Standard'
    
    /***************************************************************************************
    Table Reset
    ----------------------------------------------------------------------------------------
    Truncate all tables to start with a fresh setup
    ***************************************************************************************/
    
    DELETE FROM [dbo].[Customers]
    DELETE FROM [dbo].[FeedCredentials]
    DELETE FROM [dbo].[FeedCurrentDocuments]
    DELETE FROM [dbo].[Feeds]
    DELETE FROM [dbo].[SearchConfiguration]
    DELETE FROM [dbo].[SearchIndex]
    DELETE FROM [dbo].[SearchIndexRequestLog]
    DELETE FROM [dbo].[SearchInsightsData]
    DELETE FROM [dbo].SearchInstanceKeys
    DELETE FROM [dbo].SearchInstances
    DELETE FROM [dbo].[Synonyms]
    DELETE FROM [dbo].[Themes]
    
    /***************************************************************************************
    Uncomment this if you want to clear down the insights
    ***************************************************************************************/
    TRUNCATE TABLE [dbo].[SearchInsightsData]
    TRUNCATE TABLE [dbo].[SearchIndexRequestLog]
    
    PRINT '********************************'
    PRINT 'Inserting Service Resource Entry'
    PRINT '********************************'
    
    INSERT INTO [dbo].[SearchInstances]
    (
		[Id],
		[ServiceName],
		[Location],
		[PricingTier],
		[Replicas],
		[Partitions],
		[IsShared],
		[Type],
		[RootEndpoint]
    )
    VALUES
    (
		@SearchInstanceId, -- or your actual Id (GUID)
		@SearchInstanceName, -- ServiceName
		@ServiceLocation, -- Location
		@AzurePricingTier, -- PricingTier
		@Replicas, -- Replicas
		@Partitions, -- Partitions
		@IsShared, -- IsShared (0 for false, 1 for true)
		'Production', -- Type
		@SearchInstanceEndpoint    -- RootEndpoint
    );
    
    
    PRINT '********************************'
    PRINT 'Inserting Search Resource Keys'
    PRINT '********************************'
    
    INSERT INTO
    [dbo].SearchInstanceKeys
    (
		Id,
		SearchInstanceId,
		KeyType,
		[Name],
		ApiKey,
		CreatedDate,
		ModifiedDate,
		IsLatest
    )
    VALUES
    (
		@SearchInstanceKeyId_AdminKey,
		@SearchInstanceId,
		'Admin',
		'Primary Admin key',
		@AdminKey,
		@Now,
		null,
		1
    )
    INSERT INTO
    [dbo].SearchInstanceKeys
    (
		Id,
		SearchInstanceId,
		KeyType,
		[Name],
		ApiKey,
		CreatedDate,
		ModifiedDate,
		IsLatest
    )
    VALUES
    (
		@SearchInstanceKeyId_SecondaryAdminKey,
		@SearchInstanceId,
		'Admin',
		'Secondary Admin key',
		@SecondaryKey,
		@Now,
		null,
    1
    )
    INSERT INTO
    [dbo].SearchInstanceKeys
    (
		Id,
		SearchInstanceId,
		KeyType,
		[Name],
		ApiKey,
		CreatedDate,
		ModifiedDate,
		IsLatest
    )
    VALUES
    (
		@SearchInstanceKeyId_QueryKey,
		@SearchInstanceId,
		'Query',
		'Query key',
		@QueryKey,
		@Now,
		null,
		1
    )
        
    PRINT '************************'
    PRINT 'Inserting Test Customers'
    PRINT '************************'
    
    PRINT '************************'
    PRINT 'Inserting Test Customer 1'
    PRINT '************************'
    
    INSERT INTO
    [dbo].Customers
    (
		[Id],
		[BusinessName],
		[CreatedDate],
		[ModifiedDate]
    )
    VALUES
    (
		@CustomerId_JGilmartin_Motors,
		@BusinessName_1,
		@Now,
		NULL
    )
    
    PRINT '********************************'
    PRINT 'Inserting Search Index for Test Customer 1'
    PRINT '********************************'
    
    INSERT INTO
    [dbo].SearchIndex
    (
		Id,
		SearchInstanceId,
		IndexName,
		FriendlyName,
		CustomerId,
		CreatedDate,
		PricingSkuId
    )
    VALUES
    (
		@SearchIndexId_1,
		@SearchInstanceId,
		@CustomerIndexName_1,
		@BusinessName_1,
		@CustomerId_JGilmartin_Motors,
		@Now,
		@SkuIdFree
    )
    
    PRINT '********************************'
    PRINT 'Inserting Feed Entry for Test Customer 1'
    PRINT '********************************'
    
    INSERT INTO [dbo].Feeds
    (
		[FeedType]
		,[FeedScheduleCron]
		,[SearchIndexId]
		,[DataFormat]
		,[CreatedDate]
		,[SupersededDate]
		,[IsLatest]
    )
    VALUES
    (
		@FeedType,
		@FeedCron,
		@SearchIndexId_1,
		@DataFormat,
		@Now,
		null,
		1
    )
    
    PRINT '********************************'
    PRINT 'Inserting Synonyms'
    PRINT '********************************'
    
    INSERT INTO [dbo].[Synonyms]
    (
		Id,
		SearchIndexId,
		KeyWord,
		SolrFormat
    )
    VALUES
    (
		@SynonymId_1,
		@SearchIndexId_1,
		@KeyWord_1,
		@SolrFormat_1
    )
    
    INSERT INTO [dbo].[Synonyms]
    (
		Id,
		SearchIndexId,
		KeyWord,
		SolrFormat
    )
    VALUES
    (
		@SynonymId_2,
		@SearchIndexId_1,
		@KeyWord_2,
		@SolrFormat_2
    )
    
    PRINT '********************************'
    PRINT 'Customer 1 - S2 Demo - Theme Details'
    PRINT '********************************'
    
    INSERT INTO [dbo].[Themes]
    (
		[Id]
		,[PrimaryHexColour]
		,[SecondaryHexColour]
		,[NavBarHexColour]
		,[LogoURL]
		,[MissingImageURL]
		,[CustomerId]
		,[SearchIndexId]
		,[CreatedDate]
		,[ModifiedDate]
    )
    VALUES
    (
		@ThemeId_1,
		@ThemePrimaryThemeColour_1,
		@ThemeSecondaryThemeColour_1,
		@ThemeNavBarColourColour_1,
		@ThemeLogoURL_1,
		@ThemeMissingImageURL_1,
		@CustomerId_JGilmartin_Motors,
		@SearchIndexId_1,
		@Now,
		NULL
    )
      
    PRINT '*******************************************'
    PRINT 'Search Index 1 FeedCredentials'
    PRINT '*******************************************'
    
    INSERT INTO [dbo].[FeedCredentials]
		([Id]
		,[SearchIndexId]
		,[Username]
		,[PasswordHash]
		,[CreatedDate]
		,[ModifiedDate])
    VALUES
		(
		@FeedId_1
		, @FeedSearchIndexId_1
		, @FeedUsername_1
		, @FeedPasswordHash_1
		, @Now
		, null
		)
    
    PRINT '****************************'
    PRINT 'Setup S2 Demo Data Configuration Mappings'
    PRINT '****************************'
    
    -- Option 1: Enable Auto Complete
    INSERT INTO [dbo].[SearchConfiguration]
		(
		[Id],
		[Value],
		[SearchIndexId],
		[Key],
		[FriendlyName],
		[Description],
		[DataType],
		[OrderIndex],
		[CreatedDate],
		[ModifiedDate]
		)
    VALUES
    (
		@SearchConfiguration_EnableAutoComplete_Id,
		'true',
		NEWID(), -- Replace with actual SearchIndexId if needed
		@SearchConfiguration_EnableAutoComplete_Key,
		@SearchConfiguration_EnableAutoComplete_FriendlyName,
		@SearchConfiguration_EnableAutoComplete_Description,
		'Boolean',
		NULL,
		GETUTCDATE(),
		NULL
    );
    
    -- Option 2: Hide Icon Vehicle Counts
    INSERT INTO [dbo].[SearchConfiguration]
    (
		[Id],
		[Value],
		[SearchIndexId],
		[Key],
		[FriendlyName],
		[Description],
		[DataType],
		[OrderIndex],
		[CreatedDate],
		[ModifiedDate]
    )
    VALUES
    (
		@SearchConfiguration_HideIconVehicleCounts_Id,
		'true',
		NEWID(), -- Replace with actual SearchIndexId if needed
		@SearchConfiguration_HideIconVehicleCounts_Key,
		@SearchConfiguration_HideIconVehicleCounts_FriendlyName,
		@SearchConfiguration_HideIconVehicleCounts_Description,
		'Boolean',
		NULL,
		GETUTCDATE(),
		NULL
    );
    
    -- Option 3: Placeholder Text Array (5 rows)
    INSERT INTO [dbo].[SearchConfiguration]
    (
		[Id],
		[Value],
		[SearchIndexId],
		[Key],
		[FriendlyName],
		[Description],
		[DataType],
		[OrderIndex],
		[CreatedDate],
		[ModifiedDate]
    )
    VALUES
    (NEWID(), @SearchConfiguration_PlaceholderText_Value_1, NEWID(), @SearchConfiguration_PlaceholderText_Key_1, 'Placeholder Text 1', 'Placeholder for search bar', 'String', 1, GETUTCDATE(), NULL),
    (NEWID(), @SearchConfiguration_PlaceholderText_Value_2, NEWID(), @SearchConfiguration_PlaceholderText_Key_2, 'Placeholder Text 2', 'Placeholder for search bar', 'String', 2, GETUTCDATE(), NULL),
    (NEWID(), @SearchConfiguration_PlaceholderText_Value_3, NEWID(), @SearchConfiguration_PlaceholderText_Key_3, 'Placeholder Text 3', 'Placeholder for search bar', 'String', 3, GETUTCDATE(), NULL),
    (NEWID(), @SearchConfiguration_PlaceholderText_Value_4, NEWID(), @SearchConfiguration_PlaceholderText_Key_4, 'Placeholder Text 4', 'Placeholder for search bar', 'String', 4, GETUTCDATE(), NULL),
    (NEWID(), @SearchConfiguration_PlaceholderText_Value_5, NEWID(), @SearchConfiguration_PlaceholderText_Key_5, 'Placeholder Text 5', 'Placeholder for search bar', 'String', 5, GETUTCDATE(), NULL);
        
    PRINT '********************************'
    PRINT 'Script - Complete'
    PRINT '********************************'
END