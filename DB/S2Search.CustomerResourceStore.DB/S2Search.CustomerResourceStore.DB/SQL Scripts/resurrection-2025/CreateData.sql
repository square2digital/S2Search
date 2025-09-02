use CustomerResourceStore
go
    /***************************************************************************************
     Static Variables
     ----------------------------------------------------------------------------------------
     These do not change
     ***************************************************************************************/
    DECLARE @Now datetime = GETUTCDATE() 
    DECLARE @Subscription_ResourceGroupsQuota int = 980 
    DECLARE @ResourceGroup_ResourceQuota int = 800

    /***************************************************************************************
     Dummy Data - Script Start
     ***************************************************************************************/
    DECLARE @InsertDummyData bit = 1 --NOTE: This will not link to a search resource in Azure, it is purely for setting up a mock instance with the expected data structure
    IF @InsertDummyData = 1 BEGIN PRINT 'Dummy Data Script - Started' --SearchIndex variables
    DECLARE @CustomerId uniqueidentifier = 'afeb217b-813a-4b9c-82ec-e0221d5e95b1'
    DECLARE @SearchInstanceId uniqueidentifier = '97032266-c1c0-4278-8816-053bbc3a1036'
    DECLARE @ResourceGroup varchar(255) = 'S2Search'
    DECLARE @ServiceLocation varchar(50) = 'West Europe'
    DECLARE @SearchInstanceEndpoint varchar(250) = 'https://s2-search-dev.search.windows.net'
	DECLARE @StorageURI varchar(250) = 'https://s2storagedev.blob.core.windows.net'
    DECLARE @AdminKey varchar(255) = 'meh33Ur6Zd7oGUv201TvZAXD5mqTOH9QN1YtFePp86AzSeDwh11h'
    DECLARE @SecondaryKey varchar(255) = 'Jc6htHDfDNK6MAgGm80ZBFqjsb0RZjkdwfSYxjMh6XAzSeAHeUua'
    DECLARE @QueryKey varchar(255) = 'JTli3f7UNsq6UP5aarwr6kEuXLziImNklC1EZlI3zSAzSeDZXlvC'
    DECLARE @Replicas int = 1
    DECLARE @Partitions int = 1
    DECLARE @IsShared bit = 1
    DECLARE @SearchInstanceName varchar(255) = 's2-search-dev'
    DECLARE @SearchIndexName varchar(255) = 'vehicles-' + LEFT(CONVERT(varchar(255), NEWID()), 8) --Feeds
    DECLARE @FeedType varchar(20) = 'FTPS'
    DECLARE @FeedCron varchar(50) = '5 * * * *'
	DECLARE @DataFormat varchar(50) = 'DMS14'
    
    --NotificationRules
    DECLARE @NotifyTransmitType varchar(255) = 'email'
    DECLARE @NotifyRecipients varchar(255) = 'notify@square2digital.com'
    DECLARE @NotifyTrigger varchar(255) = 'Feed_Success'
    
    --SearchInterfaces
    DECLARE @InterfaceType varchar(50) = 'API_Consumption'
    DECLARE @InterfaceLogoURL varchar(255) = NULL
    DECLARE @IntefaceBannerStyle varchar(255) = NULL
    
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
	-- Test User -> Harley Gilmartin - user of Harley Motors
	-- Test User -> Harper Gilmartin - user of Haper Motors
	DECLARE @Azure_B2C_User_Jonathan_Gilmartin uniqueidentifier = 'a870f617-c469-4fdc-b76d-98a990577583'
	DECLARE @Azure_B2C_User_Harley_Gilmartin uniqueidentifier = 'db3e8082-4aa5-4622-84d3-837c72e389b8'	
	DECLARE @Azure_B2C_User_Harper_Gilmartin uniqueidentifier = '0a0cc61e-f58c-4f84-bff5-faf0bc3b0bb6'

	DECLARE @CustomerId_JGilmartin_Motors uniqueidentifier = @Azure_B2C_User_Jonathan_Gilmartin
	DECLARE @CustomerId_Harley_Motors uniqueidentifier = @Azure_B2C_User_Harley_Gilmartin
	DECLARE @CustomerId_Harper_Motors uniqueidentifier = @Azure_B2C_User_Harper_Gilmartin
		
	-- ********************************************
	-- ** S2-Demo Endpoints
	-- ********************************************	
	DECLARE @S2DemoEndpoint varchar(100) = 'demo.s2search.co.uk'                        -- BLUE - Corporate colours for Square 2 Digital
	--DECLARE @JGilmartinMotorsEndpoint varchar(100) = 'jgilmartinmotors.s2search.co.uk'  -- Green
	DECLARE @HarleyMotorsEndpoint varchar(100) = 'harleygilmartinmotors.s2search.co.uk' -- PURPLE
	DECLARE @HarperMotorsEndpoint varchar(100) = 'harpergilmartinmotors.s2search.co.uk' -- PURPLE
	
	-- endpoint overrides
	DECLARE @LocalK8sEndpoint varchar(100) = 'localhost:3000'
	DECLARE @LocalDevEndpoint varchar(100) = 'localhost:2997' 	

	-- ********************************************
	-- use this to override endpoints - useful to setting a search instance to another URL or to localhost
	-- ********************************************		    
	SET @S2DemoEndpoint = @LocalK8sEndpoint
	SET @HarleyMotorsEndpoint = @LocalDevEndpoint
	--SET @HarleyMotorsEndpoint = @LocalK8sEndpoint

	-- ************************
    -- S2 Demo  - customer 1
	-- ************************    
    DECLARE @BusinessName_1 varchar(100) = 'S2 Demo'
	DECLARE @CustomerIndexName_1 varchar(100) = 's2-demo-vehicles'
	DECLARE @SearchIndexId_1 uniqueidentifier = '8c663063-4217-4f54-973f-8faec6131b5b'
	DECLARE @ThemeId_1 uniqueidentifier = '8c663063-4217-4f54-973f-8faec6131b5b' 
    DECLARE @ThemeLogoURL_1 varchar(1000) = @StorageURI + '/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg'
	DECLARE @ThemeMissingImageURL_1 varchar(1000) = @StorageURI + '/assets/image-coming-soon.jpg' 
    DECLARE @ThemePrimaryThemeColour_1 varchar(10) = '#006bd1'
    DECLARE @ThemeSecondaryThemeColour_1 varchar(10) = '#003c75'
	DECLARE @ThemeNavBarColourColour_1 varchar(10) = '#006bd1'   

	-- ******************
    -- harley gilmartin motors  - customer 2
	-- ******************    
    DECLARE @BusinessName_2 varchar(100) = 'Harley Motors'
	DECLARE @CustomerIndexName_2 varchar(100) = 'harley-motors-vehicles'
	DECLARE @SearchIndexId_2 uniqueidentifier = '4cdd1c8a-30f4-4bc7-8de4-3f01ca7bc97f'
	DECLARE @ThemeId_2 uniqueidentifier = '3f62e01c-4ccf-40bb-9bfb-50372d18c977' 
	DECLARE @ThemeLogoURL_2 varchar(1000) = @StorageURI + '/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg'
	DECLARE @ThemeMissingImageURL_2 varchar(1000) = @StorageURI + '/assets/image-coming-soon.jpg' 
	DECLARE @ThemePrimaryThemeColour_2 varchar(10) = '#71538f'
	DECLARE @ThemeSecondaryThemeColour_2 varchar(10) = '#3e056e'
	DECLARE @ThemeNavBarColourColour_2 varchar(10) = '#7e3cb5'

	-- ******************
    -- Harper motors  - customer 3
	-- ******************    
    DECLARE @BusinessName_3 varchar(100) = 'Harper Motors'
	DECLARE @CustomerIndexName_3 varchar(100) = 'harper-motors-vehicles'
	DECLARE @SearchIndexId_3 uniqueidentifier = 'e30f4cb5-dbca-450f-a325-8e9bf2ba232b'
	DECLARE @ThemeId_3 uniqueidentifier = '5b0f357e-4fbf-466c-9f2b-db93fd1fd5ec' 
    DECLARE @ThemeLogoURL_3 varchar(1000) = @StorageURI + '/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg' 
	DECLARE @ThemeMissingImageURL_3 varchar(1000) = @StorageURI + '/assets/image-coming-soon.jpg' 
	DECLARE @ThemePrimaryThemeColour_3 varchar(10) = '#006bd1'
    DECLARE @ThemeSecondaryThemeColour_3 varchar(10) = '#003c75'
	DECLARE @ThemeNavBarColourColour_3 varchar(10) = '#006bd1'
   
    /***************************************************************************************
     Required Variables
     ----------------------------------------------------------------------------------------
     You will need to set these variables for the script to run successfully.
     See the README for instructions on how to set up a ServicePrinciple in Azure
     ***************************************************************************************/
    DECLARE @SubscriptionId uniqueidentifier = 'f8cff945-b5e5-462a-9786-d69bd7a0eb34'
    DECLARE @SubscriptionName varchar(50) = 'S2-Pay-As-You-Go'
    DECLARE @ServicePrinciple_Name varchar(50) = 'TestProvision'
    DECLARE @ServicePrinciple_ClientId uniqueidentifier = '5ba3c688-71f9-474a-952f-c7b527a29f65' 
    DECLARE @ServicePrinciple_TenantId uniqueidentifier = '0694e993-9911-4269-b7e2-bfd29ca197c6' 
    DECLARE @ServicePrinciple_ClientKeyName varchar(50) = 'test' 
    DECLARE @ServicePrinciple_ClientKeySecret varchar(100) = '64ZZWyvGfVC~A1e.YF9IjQF.kXx-3hfOn4' 
    DECLARE @ServicePrinciple_ClientKeyExpiryDate datetime = '2021-11-11 00:00' 
    DECLARE @AzurePricingTier varchar(50) = 'Free'

     /***************************************************************************************
     Customer Pricing Tiers
     ***************************************************************************************/
     DECLARE @SkuIdFree varchar(50) = 'FREE'
     DECLARE @SkuIdFreeName varchar(100) = 'S2 Free Tier'
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

	 DECLARE @FeedSearchIndexId_2 uniqueidentifier = @SearchIndexId_2
	 DECLARE @FeedId_2 uniqueidentifier = '9875c5ef-c652-47fd-be7d-6eb9cc74723a'
	 DECLARE @FeedUsername_2 varchar(100) = 'jgilmartin_FTP_2'
     DECLARE @FeedPasswordHash_2 varchar(255) = 'PPk57p7jPXbVQBAK'

	 DECLARE @FeedSearchIndexId_3 uniqueidentifier = @SearchIndexId_3
	 DECLARE @FeedId_3 uniqueidentifier = '2465c76a-ec51-4d3c-9695-834312f154df'
	 DECLARE @FeedUsername_3 varchar(100) = 'dpmotors_FTP_3'
     DECLARE @FeedPasswordHash_3 varchar(255) = '3Hcprz6vvK2nMd4qd4HmP'

    /***************************************************************************************
     Table Reset
     ----------------------------------------------------------------------------------------
     Truncate all tables to start with a fresh setup
     ***************************************************************************************/

DELETE FROM
    dbo.Themes
DELETE FROM
    dbo.DeletedSearchIndexConfiguration
DELETE FROM
    [dbo].[FeedCredentials]
DELETE FROM
    dbo.NotificationRules
DELETE FROM
    dbo.Notifications
DELETE FROM
    dbo.SearchInterfaces
DELETE FROM
    dbo.[Synonyms]
DELETE FROM
    dbo.GenericSynonyms
DELETE FROM
    dbo.Feeds
DELETE FROM
	[dbo].[SearchConfigurationMappings]
DELETE FROM
	[dbo].[SearchConfigurationOptions]
DELETE FROM
	[dbo].[SearchConfigurationDataTypes]
DELETE FROM
    dbo.SearchIndexKeys
DELETE FROM
    dbo.SearchIndex
DELETE FROM
    dbo.SearchInstanceKeys
DELETE FROM
    dbo.SearchInstanceCapacity
DELETE FROM
    dbo.SearchInstanceReservations
DELETE FROM
    dbo.SearchInstances
DELETE FROM
    dbo.ResourceGroupsCapacity
DELETE FROM
    dbo.ResourceGroups
DELETE FROM
    dbo.SubscriptionCapacity
DELETE FROM
    dbo.SubscriptionResourceCapacity
DELETE FROM
    dbo.ClientKeys
DELETE FROM
    dbo.ServicePrinciples
DELETE FROM
    dbo.Subscriptions
DELETE FROM
    dbo.Customers
DELETE FROM 
	[dbo].[CustomerPricing]
DELETE FROM
	[dbo].[CustomerPricingTiers]

/***************************************************************************************
Uncomment this if you want to clear down the insights
***************************************************************************************/
--TRUNCATE TABLE [insights].[SearchInsightsData]
--TRUNCATE TABLE [insights].[SearchIndexRequestLog]

/***************************************************************************************
Initial Setup - Script Start
***************************************************************************************/
PRINT '****************************'
PRINT 'Setup Configuration Options'
PRINT '****************************'

INSERT INTO [dbo].[SearchConfigurationDataTypes]
		([SearchConfigurationDataTypeId],
		[DataType])
	VALUES
		(@SearchConfigurationID_Bool, 'Bool'),
		(@SearchConfigurationID_Int, 'Int'),
		(@SearchConfigurationID_String, 'String'),
		(@SearchConfigurationID_Decimal, 'Decimal'),
		(@SearchConfigurationID_Array, 'Array')

INSERT INTO [dbo].[SearchConfigurationOptions]
        ([SeachConfigurationOptionId]
        ,[Key]
        ,[FriendlyName]
        ,[Description]
		,[SearchConfigurationDataTypeId]
		,[OrderIndex]
        ,[CreatedDate]
		,[ModifiedDate])
    VALUES
        (@SearchConfigurationOption_EnableAutoComplete_Id
        ,@SearchConfigurationOption_EnableAutoComplete_Key
        ,@SearchConfigurationOption_EnableAutoComplete_FriendlyName
        ,@SearchConfigurationOption_EnableAutoComplete_Description
		,@SearchConfigurationID_Bool
		,null
        ,@Now
		,null)

INSERT INTO [dbo].[SearchConfigurationOptions]
        ([SeachConfigurationOptionId]
        ,[Key]
        ,[FriendlyName]
        ,[Description]
		,[SearchConfigurationDataTypeId]
		,[OrderIndex]
        ,[CreatedDate]
		,[ModifiedDate])
    VALUES
        (@SearchConfigurationOption_HideIconVehicleCounts_Id
        ,@SearchConfigurationOption_HideIconVehicleCounts_Key
        ,@SearchConfigurationOption_HideIconVehicleCounts_FriendlyName
        ,@SearchConfigurationOption_HideIconVehicleCounts_Description
		,@SearchConfigurationID_Bool
		,null        
		,@Now
		,null)

-- **************************
-- Place holder text entries
-- **************************
INSERT INTO [dbo].[SearchConfigurationOptions]
        ([SeachConfigurationOptionId]
        ,[Key]
        ,[FriendlyName]
        ,[Description]
		,[SearchConfigurationDataTypeId]
		,[OrderIndex]
        ,[CreatedDate]
		,[ModifiedDate])
    VALUES
        (@SearchConfigurationOption_PlaceholderText_Id_1
        ,@SearchConfigurationOption_PlaceholderText_Key_1
        ,@SearchConfigurationOption_PlaceholderText_FriendlyName
        ,'Placeholder Text 1'
		,@SearchConfigurationID_String
        ,1
		,@Now
		,null)

INSERT INTO [dbo].[SearchConfigurationOptions]
        ([SeachConfigurationOptionId]
        ,[Key]
        ,[FriendlyName]
        ,[Description]
		,[SearchConfigurationDataTypeId]
		,[OrderIndex]
        ,[CreatedDate]
		,[ModifiedDate])
    VALUES
        (@SearchConfigurationOption_PlaceholderText_Id_2
        ,@SearchConfigurationOption_PlaceholderText_Key_2
        ,@SearchConfigurationOption_PlaceholderText_FriendlyName
        ,'Placeholder Text 2'
		,@SearchConfigurationID_String
        ,2
		,@Now
		,null)

INSERT INTO [dbo].[SearchConfigurationOptions]
        ([SeachConfigurationOptionId]
        ,[Key]
        ,[FriendlyName]
        ,[Description]
		,[SearchConfigurationDataTypeId]
		,[OrderIndex]
        ,[CreatedDate]
		,[ModifiedDate])
    VALUES
        (@SearchConfigurationOption_PlaceholderText_Id_3
        ,@SearchConfigurationOption_PlaceholderText_Key_3
        ,@SearchConfigurationOption_PlaceholderText_FriendlyName
        ,'Placeholder Text 3'
		,@SearchConfigurationID_String
        ,3
		,@Now
		,null)

INSERT INTO [dbo].[SearchConfigurationOptions]
        ([SeachConfigurationOptionId]
        ,[Key]
        ,[FriendlyName]
        ,[Description]
		,[SearchConfigurationDataTypeId]
		,[OrderIndex]
        ,[CreatedDate]
		,[ModifiedDate])
    VALUES
        (@SearchConfigurationOption_PlaceholderText_Id_4
        ,@SearchConfigurationOption_PlaceholderText_Key_4
        ,@SearchConfigurationOption_PlaceholderText_FriendlyName
        ,'Placeholder Text 4'
		,@SearchConfigurationID_String
        ,4
		,@Now
		,null)

INSERT INTO [dbo].[SearchConfigurationOptions]
        ([SeachConfigurationOptionId]
        ,[Key]
        ,[FriendlyName]
        ,[Description]
		,[SearchConfigurationDataTypeId]
		,[OrderIndex]
        ,[CreatedDate]
		,[ModifiedDate])
    VALUES
        (@SearchConfigurationOption_PlaceholderText_Id_5
        ,@SearchConfigurationOption_PlaceholderText_Key_5
        ,@SearchConfigurationOption_PlaceholderText_FriendlyName
        ,'Placeholder Text 5'
		,@SearchConfigurationID_String
        ,5
		,@Now
		,null)
	
INSERT INTO
    dbo.Subscriptions (SubscriptionId, [Name])
VALUES
    (@SubscriptionId, @SubscriptionName)
INSERT INTO
    dbo.SubscriptionCapacity (
        SubscriptionId,
        ResourceGroupsQuota,
        ResourceGroupsUsed,
        ResourceGroupsAvailable,
        ModifiedDate
    )
VALUES
    (
        @SubscriptionId,
        @Subscription_ResourceGroupsQuota,
        0,
        @Subscription_ResourceGroupsQuota,
        @Now
    )
INSERT INTO
    dbo.SubscriptionResourceCapacity (
        SubscriptionId,
        PricingTier,
        ResourceType,
        Quota,
        Available,
        Used,
        ModifiedDate
    )
VALUES
    (
        @SubscriptionId,
        @AzurePricingTier,
        'Azure Cognitive Search',
        1,
        1,
        0,
        @Now
    )
INSERT INTO
    dbo.ServicePrinciples (
        ClientId,
        SubscriptionId,
        [Name],
        TenantId
    )
VALUES
    (
        @ServicePrinciple_ClientId,
        @SubscriptionId,
        @ServicePrinciple_Name,
        @ServicePrinciple_TenantId
    )
INSERT INTO
    dbo.ClientKeys (ClientId, [Name], [Value], ExpiryDate)
VALUES
    (
        @ServicePrinciple_ClientId,
        @ServicePrinciple_ClientKeyName,
        @ServicePrinciple_ClientKeySecret,
        @ServicePrinciple_ClientKeyExpiryDate
    )

INSERT INTO 
    dbo.CustomerPricingTiers (SkuId, [Name], [Description], EffectiveFromDate, EffectiveToDate)
VALUES
    (
    @SkuIdFree,
    @SkuIdFreeName,
    @SkuIdFreeDescription,
    @SkuIdFreeEffectiveFrom,
    NULL
    ),
    (
    @SkuIdFreeTrial,
    @SkuIdFreeTrialName,
    @SkuIdFreeTrialDescription,
    @SkuIdFreeTrialEffectiveFrom,
    @SkuIdFreeTrialEffectiveTo
    )

INSERT INTO
    dbo.CustomerPricing (SkuId, Price, EffectiveFromDate, EffectiveToDate)
VALUES
    (
    @SkuIdFree,
    0,
    @SkuIdFreeEffectiveFrom,
    NULL
    ),
    (
    @SkuIdFreeTrial,
    0,
    @SkuIdFreeTrialEffectiveFrom,
    @SkuIdFreeTrialEffectiveTo
    )
    /***************************************************************************************
     Initial Setup - Script End
     ***************************************************************************************/
    PRINT 'Inserting Resource Group'

INSERT INTO
    dbo.ResourceGroups (ResourceGroup, SubscriptionId)
VALUES
    (@ResourceGroup, @SubscriptionId) PRINT 'Inserting Resource Group Capacity'
INSERT INTO
    dbo.ResourceGroupsCapacity (
        ResourceGroup,
        ResourcesQuota,
        ResourcesUsed,
        ResourcesAvailable,
        ModifiedDate
    )
VALUES
    (
        @ResourceGroup,
        @ResourceGroup_ResourceQuota,
        1,
        @ResourceGroup_ResourceQuota - 1,
        @Now
    )
	
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
	PRINT 'Inserting Search Resource Keys - '
	PRINT '********************************'

INSERT INTO
    dbo.SearchInstanceKeys (
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
    dbo.SearchInstanceKeys (
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
    dbo.SearchInstanceKeys (
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
	PRINT 'Inserting Test Customer 1 - '
	PRINT '************************'

	INSERT INTO
		dbo.Customers (
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
	PRINT 'Inserting Search Index for Test Customer 1 - '
	PRINT '********************************'

	INSERT INTO
		dbo.SearchIndex (
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
	PRINT 'Search Index Keys for Test Customer 1 - '
	PRINT '********************************'

	INSERT INTO
		dbo.SearchIndexKeys (
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
	PRINT 'Inserting Feed Entry for Test Customer 1 - '
	PRINT '********************************'

	INSERT INTO
		dbo.Feeds (
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
	PRINT 'Inserting Test Customer 2 - '
	PRINT '********************************'

	INSERT INTO
		dbo.Customers (
			[CustomerId],
			[BusinessName],
			[CreatedDate],
			[ModifiedDate]
		)
	VALUES
		(
			@CustomerId_Harley_Motors,
			@BusinessName_2,
			@Now,
			NULL
		) 
	
	PRINT '********************************'
	PRINT 'Inserting Search Index for Test Customer 2 - '
	PRINT '********************************'

	INSERT INTO
		dbo.SearchIndex (
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
			@SearchIndexId_2,
			@SearchInstanceId,
			@CustomerIndexName_2,
			@BusinessName_2,
			@CustomerId_Harley_Motors,
			@Now,
			@SkuIdFree
		) 
	
	PRINT '********************************'
	PRINT 'Search Index Keys for Test Customer 2 - '
	PRINT '********************************'

	INSERT INTO
		dbo.SearchIndexKeys (
			[SearchIndexId],
			[Name],
			[SearchInstanceKeyId],
			[CreatedDate]
		)
	VALUES
		(
			@SearchIndexId_2,
			@BusinessName_2 + ' test key',
			@SearchInstanceKeyId_QueryKey,
			@Now
		) 
	
	PRINT '********************************'
	PRINT 'Inserting Feed Entry for Test Customer 2 - '
	PRINT '********************************'

	INSERT INTO
		dbo.Feeds (
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
			@SearchIndexId_2,
			@DataFormat,
			@Now,
			null,
			1
		) 
	
	PRINT '********************************'
	PRINT 'Inserting Test Customer 3 - '
	PRINT '********************************'

	INSERT INTO
		dbo.Customers (
			[CustomerId],
			[BusinessName],
			[CreatedDate],
			[ModifiedDate]
		)
	VALUES
		(
			@CustomerId_Harper_Motors,
			@BusinessName_3,
			@Now,
			NULL
		) 
	
	PRINT '********************************'
	PRINT 'Inserting Search Index for Test Customer 3 - '
	PRINT '********************************'

	INSERT INTO
		dbo.SearchIndex (
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
			@SearchIndexId_3,
			@SearchInstanceId,
			@CustomerIndexName_3,
			@BusinessName_3,
			@CustomerId_Harper_Motors,
			@Now,
			@SkuIdFree
		) 
	
	PRINT '********************************'
	PRINT 'Search Index Keys for Test Customer 3 - '
	PRINT '********************************'

	INSERT INTO
		dbo.SearchIndexKeys (
			[SearchIndexId],
			[Name],
			[SearchInstanceKeyId],
			[CreatedDate]
		)
	VALUES
		(
			@SearchIndexId_3,
			@BusinessName_3 + ' test key',
			@SearchInstanceKeyId_QueryKey,
			@Now
		) 
	
	PRINT '********************************'
	PRINT 'Inserting Feed Entry for Test Customer 3 - '
	PRINT '********************************'

	INSERT INTO
		dbo.Feeds (
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
			@SearchIndexId_3,
			@DataFormat,
			@Now,
			null,
			1
		) 	


	PRINT '********************************'
	PRINT 'Inserting Notification Rule Entry'
	PRINT '********************************'

INSERT INTO
    dbo.NotificationRules (
        SearchIndexId,
        TransmitType,
        Recipients,
        [Trigger]
    )
VALUES
    (
        @SearchIndexId_1,
        @NotifyTransmitType,
        @NotifyRecipients,
        @NotifyTrigger
    ) 
	
	PRINT '********************************'
	PRINT 'Inserting Search Interface Entry'
	PRINT '********************************'


-- *********************************************************************
-- Local host Interfaces - these are what bind to the 2 test Search Endpoints
-- *********************************************************************

-- *********************************
-- S2 Demo data - Azure Demo Environment
-- *********************************
INSERT INTO
    dbo.SearchInterfaces (
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
		@IntefaceBannerStyle,
		@Now,
		NULL,
		1
    )

-- *********************************
-- harley motors data - Local K8s
-- *********************************
INSERT INTO
    dbo.SearchInterfaces (
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
        @SearchIndexId_2,
		@HarleyMotorsEndpoint,
        @InterfaceType,
        @InterfaceLogoURL,
		@IntefaceBannerStyle,
		@Now,
		NULL,
		1
    )
	

-- *********************************
-- harper motors data - Local K8s
-- *********************************
INSERT INTO
    dbo.SearchInterfaces (
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
        @SearchIndexId_3,
		@HarperMotorsEndpoint,
        @InterfaceType,
        @InterfaceLogoURL,
		@IntefaceBannerStyle,
		@Now,
		NULL,
		1
    )


	PRINT '********************************'
	PRINT 'Inserting Synonyms'
	PRINT '********************************'

INSERT INTO
    dbo.[Synonyms] (
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
    dbo.[Synonyms] (
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

    INSERT INTO dbo.GenericSynonyms 
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

	PRINT '********************************'
	PRINT 'Customer 2 - harley motors - Theme Details'
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
		@ThemeId_2,
        @ThemePrimaryThemeColour_2,
        @ThemeSecondaryThemeColour_2,
        @ThemeNavBarColourColour_2,
		@ThemeLogoURL_2,
		@ThemeMissingImageURL_2,
        @CustomerId_Harley_Motors,
        @SearchIndexId_2,
		@Now,
		NULL
	)

	PRINT '********************************'
	PRINT 'Customer 3 - Harper Motors - Theme Details'
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
		@ThemeId_3,
        @ThemePrimaryThemeColour_3,
        @ThemeSecondaryThemeColour_3,
        @ThemeNavBarColourColour_3,
		@ThemeLogoURL_3,
		@ThemeMissingImageURL_3,
        @CustomerId_Harper_Motors,
        @SearchIndexId_3,
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

	PRINT '*******************************************'
	PRINT 'Search Index 2 FeedCredentials - additional index'
	PRINT '*******************************************'

	INSERT INTO [dbo].[FeedCredentials]
			   ([Id]
			   ,[SearchIndexId]
			   ,[Username]
			   ,[PasswordHash]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (@FeedId_2
			   ,@FeedSearchIndexId_2
			   ,@FeedUsername_2
			   ,@FeedPasswordHash_2
			   ,@Now
			   ,null)

	PRINT '*************************************************'
	PRINT 'Search Index 3 FeedCredentials - additional index'
	PRINT '*************************************************'

	INSERT INTO [dbo].[FeedCredentials]
			   ([Id]
			   ,[SearchIndexId]
			   ,[Username]
			   ,[PasswordHash]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (@FeedId_3
			   ,@FeedSearchIndexId_3
			   ,@FeedUsername_3
			   ,@FeedPasswordHash_3
			   ,@Now
			   ,null)

	PRINT '**********************************************'
	PRINT 'Setup S2 Demo Data Configuration Mappings'	
	PRINT '**********************************************'

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (@SearchConfigurationMapping_5
			   ,'true'
			   ,@SearchConfigurationOption_EnableAutoComplete_Id
			   ,@SearchIndexId_1
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,'false'
			   ,@SearchConfigurationOption_HideIconVehicleCounts_Id
			   ,@SearchIndexId_1
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_1
			   ,@SearchConfigurationOption_PlaceholderText_Id_1
			   ,@SearchIndexId_1
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_2
			   ,@SearchConfigurationOption_PlaceholderText_Id_2
			   ,@SearchIndexId_1
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_3
			   ,@SearchConfigurationOption_PlaceholderText_Id_3
			   ,@SearchIndexId_1
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_4
			   ,@SearchConfigurationOption_PlaceholderText_Id_4
			   ,@SearchIndexId_1
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_5
			   ,@SearchConfigurationOption_PlaceholderText_Id_5
			   ,@SearchIndexId_1
			   ,@Now
			   ,null)

	PRINT '*************************************************'
	PRINT 'Setup harley motors Configuration Mappings'	
	PRINT '*************************************************'

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,'true'
			   ,@SearchConfigurationOption_EnableAutoComplete_Id
			   ,@SearchIndexId_2
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,'false'
			   ,@SearchConfigurationOption_HideIconVehicleCounts_Id
			   ,@SearchIndexId_2
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_1
			   ,@SearchConfigurationOption_PlaceholderText_Id_1
			   ,@SearchIndexId_2
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_2
			   ,@SearchConfigurationOption_PlaceholderText_Id_2
			   ,@SearchIndexId_2
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_3
			   ,@SearchConfigurationOption_PlaceholderText_Id_3
			   ,@SearchIndexId_2
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_4
			   ,@SearchConfigurationOption_PlaceholderText_Id_4
			   ,@SearchIndexId_2
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_5
			   ,@SearchConfigurationOption_PlaceholderText_Id_5
			   ,@SearchIndexId_2
			   ,@Now
			   ,null)

	PRINT '**********************************************'
	PRINT 'Setup harper motors Configuration Mappings'	
	PRINT '**********************************************'

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (@SearchConfigurationMapping_3
			   ,'true'
			   ,@SearchConfigurationOption_EnableAutoComplete_Id
			   ,@SearchIndexId_3
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,'false'
			   ,@SearchConfigurationOption_HideIconVehicleCounts_Id
			   ,@SearchIndexId_3
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_1
			   ,@SearchConfigurationOption_PlaceholderText_Id_1
			   ,@SearchIndexId_3
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_2
			   ,@SearchConfigurationOption_PlaceholderText_Id_2
			   ,@SearchIndexId_3
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_3
			   ,@SearchConfigurationOption_PlaceholderText_Id_3
			   ,@SearchIndexId_3
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_4
			   ,@SearchConfigurationOption_PlaceholderText_Id_4
			   ,@SearchIndexId_3
			   ,@Now
			   ,null)

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (NEWID() 
			   ,@SearchConfigurationOption_PlaceholderText_Text_5
			   ,@SearchConfigurationOption_PlaceholderText_Id_5
			   ,@SearchIndexId_3
			   ,@Now
			   ,null)


	PRINT '********************************'
	PRINT 'Dummy Data Script - Complete'
	PRINT '********************************'
END
/***************************************************************************************
 Dummy Data - Script End
 ***************************************************************************************/