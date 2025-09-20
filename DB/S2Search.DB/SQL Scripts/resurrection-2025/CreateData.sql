USE S2_Search
GO
    /***************************************************************************************
     Static Variables
     ***************************************************************************************/
    DECLARE @Now datetime = GETUTCDATE() 
    
    /***************************************************************************************
     Dummy Data - Script Start
     ***************************************************************************************/
    DECLARE @InsertDummyData bit = 1 --NOTE: This will not link to a search resource in Azure, it is purely for setting up a mock instance with the expected data structure
    IF @InsertDummyData = 1 BEGIN PRINT 'Dummy Data Script - Started' --SearchIndex variables
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
    DECLARE @SearchIndexName varchar(255) = 'vehicles-' + LEFT(CONVERT(varchar(255), NEWID()), 8) --Feeds
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

    --Generic Synonyms
    DECLARE @GenericSynonymCategory varchar(50) = 'vehicles'
    DECLARE @GenericSynonymId_1 uniqueidentifier = '45AD8A77-AF90-441F-8946-BAB6DF07B3A0'
    DECLARE @GenericSolrFormat_1 varchar(250) = 'beema, bimma => BMW'

	DECLARE @GenericSynonymId_2 uniqueidentifier = 'A2BEF053-6787-4679-AB9F-0686E54313B6'
    DECLARE @GenericSolrFormat_2 varchar(250) = 'VW => Volkswagen'

    --SearchInstance Keys
    DECLARE @SearchInstanceKeyId_AdminKey uniqueidentifier = '317e8b8a-c274-499c-a4fb-bacdf8d25e81'
    DECLARE @SearchInstanceKeyId_SecondaryAdminKey uniqueidentifier = '3e0217e5-38be-4616-a81d-c1693c83aa5c'
    DECLARE @SearchInstanceKeyId_QueryKey uniqueidentifier = 'fea6648a-f781-4ec0-b8f6-ecf26eb820ac'
    
	-- **************
    -- Configuration
    -- **************

	-- Configurations Option 1 - Enable Auto Complete
	DECLARE @SearchConfigurationOption_EnableAutoComplete_Id uniqueidentifier = '4d833bb7-c52b-49ed-b42d-84a9814b9274'	
	DECLARE @SearchConfigurationOption_EnableAutoComplete_Key varchar(150) = 'EnableAutoComplete'
	DECLARE @SearchConfigurationOption_EnableAutoComplete_FriendlyName varchar(500) = 'Enable Auto Complete'
	DECLARE @SearchConfigurationOption_EnableAutoComplete_Description varchar(MAX) = 'if true, this will configure the search bar to include auto complete suggestions based on the users text inputs'

	-- Configurations Option 2 - Hide Icon Vehicle Counts
	DECLARE @SearchConfigurationOption_HideIconVehicleCounts_Id uniqueidentifier = 'd4d0978e-980c-4237-be79-f2e0ffc0ae06'	
	DECLARE @SearchConfigurationOption_HideIconVehicleCounts_Key varchar(150) = 'HideIconVehicleCounts'
	DECLARE @SearchConfigurationOption_HideIconVehicleCounts_FriendlyName varchar(500) = 'Hide Icon Vehicle Counts'
	DECLARE @SearchConfigurationOption_HideIconVehicleCounts_Description varchar(MAX) = 'if true, this will hide the icon vehicle counts on the top nav bar'

	-- Configurations Option 3 - Placeholder Text Array
	DECLARE @SearchConfigurationOption_PlaceholderText_Id_1 uniqueidentifier = '377b975f-9dcc-427b-980a-1cb5e0575007'	
	DECLARE @SearchConfigurationOption_PlaceholderText_Id_2 uniqueidentifier = '6af7989f-0554-40b9-a0c7-aa55bcbf7556'	
	DECLARE @SearchConfigurationOption_PlaceholderText_Id_3 uniqueidentifier = 'd32b4e81-641d-4f0d-ac30-a13d89ed1ae8'	
	DECLARE @SearchConfigurationOption_PlaceholderText_Id_4 uniqueidentifier = '8b949e1e-04de-4b24-9080-591956f2d3d0'	
	DECLARE @SearchConfigurationOption_PlaceholderText_Id_5 uniqueidentifier = '5cc0752a-f449-457c-93fd-29576860a6d4'	

	DECLARE @SearchConfigurationOption_PlaceholderText_Key_1 varchar(150) = 'PlaceholderText_1'
	DECLARE @SearchConfigurationOption_PlaceholderText_Key_2 varchar(150) = 'PlaceholderText_2'
	DECLARE @SearchConfigurationOption_PlaceholderText_Key_3 varchar(150) = 'PlaceholderText_3'
	DECLARE @SearchConfigurationOption_PlaceholderText_Key_4 varchar(150) = 'PlaceholderText_4'
	DECLARE @SearchConfigurationOption_PlaceholderText_Key_5 varchar(150) = 'PlaceholderText_5'
	
	DECLARE @SearchConfigurationOption_PlaceholderText_FriendlyName varchar(500) = 'Enter the text you would like to appear in the search box'
	DECLARE @SearchConfigurationOption_PlaceholderText_Description varchar(MAX) = 'You can enter up to 5 different placeholders. They will be displayed in a cyclic loop on the search bar. For best results, ensure your placeholders text matches the profile of your stock.'

	DECLARE @Separator varchar(50) = '...     '

	DECLARE @SearchConfigurationOption_PlaceholderText_Text_1 varchar(250) = CONCAT('Mercedes Benz S63 AMG', @Separator)
	DECLARE @SearchConfigurationOption_PlaceholderText_Text_2 varchar(250) = CONCAT('Porsche Green GT3RS', @Separator)
	DECLARE @SearchConfigurationOption_PlaceholderText_Text_3 varchar(250) = CONCAT('BMW 7 series Silver', @Separator)
	DECLARE @SearchConfigurationOption_PlaceholderText_Text_4 varchar(250) = CONCAT('Lexus LS460 Black', @Separator)
	DECLARE @SearchConfigurationOption_PlaceholderText_Text_5 varchar(250) = CONCAT('Honda Type R Red', @Separator)
	
	--Search Configuration Mappings VALUES
	DECLARE @SearchConfigurationMapping_1 uniqueidentifier = '4350fbfb-5b7d-4162-92aa-2a3ec39a4a5f'
	DECLARE @SearchConfigurationMapping_2 uniqueidentifier = '0d4cb168-5bb9-4e4d-8a18-d39afa249eb6'
	DECLARE @SearchConfigurationMapping_3 uniqueidentifier = 'c0fd3d12-294c-41b0-b3ae-ae82cb6b5ca1'
	DECLARE @SearchConfigurationMapping_4 uniqueidentifier = '74a225fd-17ab-421a-a69c-662c2f9a9195'
	DECLARE @SearchConfigurationMapping_5 uniqueidentifier = '0ad0b4cd-65bc-4f11-bcb7-d19e98185d72'

	-- SearchIndexId is set as S2 Demo
    DECLARE @SearchConfigurationID_Bool uniqueidentifier = '9ac59cb9-ce37-4209-95c5-3c5283922ff3'
	DECLARE @SearchConfigurationID_Int uniqueidentifier = '4c8d4535-9824-4a9f-9791-8f1cf2ec280e'
	DECLARE @SearchConfigurationID_String uniqueidentifier = '679eefca-e1d5-4718-8d3c-41e270b8de1c'
	DECLARE @SearchConfigurationID_Decimal uniqueidentifier = 'd3d1488d-3c4b-4100-80e7-8db70f756957'
	DECLARE @SearchConfigurationID_Array uniqueidentifier = '5d8f0db9-d691-4ebf-ba1e-e38c2c514303'

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
	DECLARE @S2DemoEndpoint varchar(100) = 'demo.s2search.co.uk' -- BLUE - Corporate colours for Square 2 Digital
	
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
     Required Variables
     ----------------------------------------------------------------------------------------
     You will need to set these variables for the script to run successfully.
     See the README for instructions on how to set up a ServicePrinciple in Azure
     ***************************************************************************************/
                                    
     /***************************************************************************************
     Customer Pricing Tiers
     ***************************************************************************************/
     DECLARE @SkuIdFree varchar(50) = 'FREE'
     DECLARE @SkuIdFreeDescription varchar(255) = 'TBD'
     DECLARE @SkuIdFreeEffectiveFrom datetime = '2021-03-24 00:00:00.000'

     DECLARE @SkuIdFreeTrial varchar(50) = 'FREETRIALMARCH'
     DECLARE @SkuIdFreeTrialName varchar(100) = 'S2 Free Trial March 2021'
     DECLARE @SkuIdFreeTrialDescription varchar(255) = 'TBD'
     DECLARE @SkuIdFreeTrialEffectiveFrom datetime = '2021-03-24 00:00:00.000'
     DECLARE @SkuIdFreeTrialEffectiveTo datetime = '2021-03-31 23:59:59.000'

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
	 DECLARE @AzurePricingTier varchar(50) = 'Free'

    /***************************************************************************************
     Table Reset
     ----------------------------------------------------------------------------------------
     Truncate all tables to start with a fresh setup
     ***************************************************************************************/

	DELETE FROM [dbo].Customers
	DELETE FROM [dbo].[FeedCredentials]
	DELETE FROM [dbo].Feeds
	DELETE FROM [dbo].GenericSynonyms
	DELETE FROM [dbo].SearchIndex
	DELETE FROM [dbo].SearchIndexKeys
	DELETE FROM [dbo].SearchInstanceCapacity
	DELETE FROM [dbo].SearchInstanceKeys
	DELETE FROM [dbo].SearchInstances
	DELETE FROM [dbo].SearchInterfaces
	DELETE FROM [dbo].[Synonyms]
	DELETE FROM [dbo].Themes

	/***************************************************************************************
	Uncomment this if you want to clear down the insights
	***************************************************************************************/
	--TRUNCATE TABLE [insights].[SearchInsightsData]
	--TRUNCATE TABLE [insights].[SearchIndexRequestLog]

	/***************************************************************************************
	Initial Setup - Script Start
	***************************************************************************************/

PRINT '********************************'
PRINT 'Inserting Service Resource Entry'
PRINT '********************************'

INSERT INTO
    dbo.SearchInstances (
        SearchInstanceId,
        ServiceName,
        SubscriptionId,
        ResourceGroup,
        [Endpoint],
        [Location],
        PricingTier,
        Replicas,
        [Partitions],
        IsShared
    )
VALUES
    (
        @SearchInstanceId,
        @SearchInstanceName,
        @SubscriptionId,
        @ResourceGroup,
        @SearchInstanceEndpoint,
        @ServiceLocation,
        @AzurePricingTier,
        @Replicas,
        @Partitions,
        @IsShared
    ) 

	PRINT '********************************'
	PRINT 'Inserting Search Resource Keys'
	PRINT '********************************'

	INSERT INTO
	[dbo].SearchInstanceKeys (
		SearchInstanceKeyId,
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
	[dbo].SearchInstanceKeys (
		SearchInstanceKeyId,
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
	[dbo].SearchInstanceKeys (
		SearchInstanceKeyId,
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
		[dbo].Customers (
			[CustomerId],
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
		[dbo].SearchIndex (
			SearchIndexId,
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
	PRINT 'Search Index Keys for Test Customer 1'
	PRINT '********************************'

	INSERT INTO
		[dbo].SearchIndexKeys (
			[SearchIndexId],
			[Name],
			[SearchInstanceKeyId],
			[CreatedDate]
		)
	VALUES
		(
			@SearchIndexId_1,
			@BusinessName_1 + ' test key',
			@SearchInstanceKeyId_QueryKey,
			@Now
		)
	
	PRINT '********************************'
	PRINT 'Inserting Feed Entry for Test Customer 1'
	PRINT '********************************'

	INSERT INTO
		[dbo].Feeds (
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
	PRINT 'Inserting Notification Rule Entry'
	PRINT '********************************'


	-- *********************************************************************
	-- Local host Interfaces - these are what bind to the 2 test Search Endpoints
	-- *********************************************************************

	-- *********************************
	-- S2 Demo data - Azure Demo Environment
	-- *********************************
	INSERT INTO
	[dbo].SearchInterfaces (
		[SearchIndexId],
		[SearchEndpoint],
		[InterfaceType],
		[LogoURL],
		[BannerStyle],
		[CreatedDate],
		[SupersededDate],
		[IsLatest]
	)
	VALUES
	(
		@SearchIndexId_1,
		@S2DemoEndpoint,
		@InterfaceType,
		@InterfaceLogoURL,
		@InterfaceBannerStyle,
		@Now,
		NULL,
		1
	)

	PRINT '********************************'
	PRINT 'Inserting Synonyms'
	PRINT '********************************'

	INSERT INTO
	[dbo].[Synonyms] (
		SynonymId,
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

	INSERT INTO
	[dbo].[Synonyms] (
		SynonymId,
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
	PRINT 'Inserting Generic Synonyms'
	PRINT '********************************'

	INSERT INTO [dbo].GenericSynonyms 
	(
		Id,
		Category,
		SolrFormat
	)
	VALUES
		(
			@GenericSynonymId_1,
			@GenericSynonymCategory,
			@GenericSolrFormat_1
		),
		(
			@GenericSynonymId_2,
			@GenericSynonymCategory,
			@GenericSolrFormat_2
		)

	PRINT '********************************'
	PRINT 'Customer 1 - S2 Demo - Theme Details'
	PRINT '********************************'

	INSERT INTO [dbo].[Themes]
	(
			[ThemeId]
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
				(@FeedId_1
				,@FeedSearchIndexId_1
				,@FeedUsername_1
				,@FeedPasswordHash_1
				,@Now
				,null)

	PRINT '********************************'
	PRINT 'Dummy Data Script - Complete'
	PRINT '********************************'
	END

	/***************************************************************************************
	Dummy Data - Script End
	***************************************************************************************/