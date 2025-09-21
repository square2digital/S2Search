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
    DECLARE @ResourceGroup varchar(255) = 'Demo-JG-UKS-AzureSearch'
    DECLARE @ServiceLocation varchar(50) = 'West Europe'
    DECLARE @SearchInstanceEndpoint varchar(250) = 'https://s2-search-basic-we.search.windows.net'
    DECLARE @AdminKey varchar(255) = '0B48F734E5DF190E69ABE88867B71943'
    DECLARE @SecondaryKey varchar(255) = '0DC8814A8220A36BCBE1C8D530D34979'
    DECLARE @QueryKey varchar(255) = 'C08EE4CB8A66CC10EFE580E6E381178B'
    DECLARE @Replicas int = 1
    DECLARE @Partitions int = 1
    DECLARE @IsShared bit = 1
    DECLARE @SearchInstanceName varchar(255) = 's2-search-basic-we'
    DECLARE @SearchIndexName varchar(255) = 'vehicles-' + LEFT(CONVERT(varchar(255), NEWID()), 8) --Feeds
    DECLARE @FeedType varchar(20) = 'FTPS'
    DECLARE @FeedCron varchar(50) = '5 * * * *'
	DECLARE @DataFormat varchar(50) = 'DMS14'
    
    --NotificationRules
    DECLARE @NotifyTransmitType varchar(255) = 'email'
    DECLARE @NotifyRecipients varchar(255) = 'notify@square2softwaredevelopment.co.uk'
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
	DECLARE @SearchConfigurationMapping_5 uniqueidentifier = 'e1017202-03c8-42f3-9d01-32c3d8583e88'

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
	-- Test User -> Malcom Dawkins - user of JGilmartin Motors
	-- Test User -> Darren Hughes - user of Saxton Vehicle Sales
	-- Test User -> Carl Bennet - user of Wearside Autoparc
	-- Test User -> James Conway - user of JGilmartin Motors
	DECLARE @Azure_B2C_User_Malcom_Dawkins uniqueidentifier = '58c7fcad-8cab-4dcc-80ae-b361d535c230'
	DECLARE @Azure_B2C_User_Darren_Hughes uniqueidentifier = 'e7099bf6-79a5-4174-b671-f766a2d438e6'
	DECLARE @Azure_B2C_User_Carl_Bennet uniqueidentifier = '46b155c0-e788-46e4-9a7b-988368c91a1f'
	DECLARE @Azure_B2C_User_James_Conway uniqueidentifier = 'd508e953-8e3c-4dea-be95-96fe1c41ee8a'
	DECLARE @Azure_B2C_User_Bradley_Stevenson uniqueidentifier = '412cfaa6-080d-4dcc-8ffc-45db0b57533d'

	DECLARE @CustomerId_Marshall_Cars uniqueidentifier = @Azure_B2C_User_Malcom_Dawkins
	DECLARE @CustomerId_Saxton_Vehicle_Data uniqueidentifier = @Azure_B2C_User_Darren_Hughes
	DECLARE @CustomerId_Wearside_Autoparc uniqueidentifier = @Azure_B2C_User_Carl_Bennet
	DECLARE @CustomerId_JGilmartin_Motors uniqueidentifier = @Azure_B2C_User_James_Conway
	DECLARE @CustomerId_DPMotors_Motors uniqueidentifier = @Azure_B2C_User_Bradley_Stevenson

	-- ********************************************
	-- ** S2-Demo Endpoints
	-- ********************************************
	--DECLARE @OktetoDevEndpoint varchar(100) = 's2searchui-service-dp-dev0.cloud.okteto.net'
	--DECLARE @AzureDevEndpoint varchar(100) = 's2search.co.uk'
	DECLARE @LocalDevEndpoint varchar(100) = 'localhost:3000' 	
	
	DECLARE @MarshallCarsEndpoint varchar(100) = 'marshall.s2search.co.uk'               -- RED
	DECLARE @SaxtonVehicleSalesEndpoint varchar(100) = 'saxton.s2search.co.uk'           -- BLUE
	DECLARE @WearsideAutoparcEndpoint varchar(100) = 'wearside.s2search.co.uk'           -- GREEN
	DECLARE @JGilmartinMotorsEndpoint varchar(100) = 'jgilmartinmotors.s2search.co.uk'   -- PURPLE
	DECLARE @DPMotorsEndpoint varchar(100) = 'dpmotors.s2search.co.uk'                   -- ORANGE

	-- ********************************************
	-- ensure this is commneted out when running on azure SQL database otherwise the callinghost will fail and the search will not work.
	-- In this example - @SaxtonVehicleSalesEndpoint - https://saxtonvehiclesales.s2search.co.uk/ would not work
	-- ********************************************	
	--SET @MarshallCarsEndpoint = @AzureDevEndpoint
	--SET @SaxtonVehicleSalesEndpoint = @OktetoDevEndpoint
	--SET @WearsideAutoparcEndpoint = @LocalDevEndpoint		
	SET @JGilmartinMotorsEndpoint = @LocalDevEndpoint
		
	-- ******************
    -- marshall-cars - customer 1
	-- ******************    
    DECLARE @BusinessName_1 varchar(100) = 'Marshall Cars' 
	DECLARE @CustomerIndexName_1 varchar(100) = 'marshall-cars-vehicles'
	DECLARE @SearchIndexId_1 uniqueidentifier = '6fb3c6d1-f480-47b5-9934-02e611aefd02' 
	DECLARE @ThemeId_1 uniqueidentifier = 'd012613b-067d-463b-87ba-6150447b7192' 
    DECLARE @ThemeLogoURL_1 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg' 
	DECLARE @ThemeMissingImageURL_1 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/customer-images/marshall-cars-test-image.jpg' 
    DECLARE @ThemePrimaryThemeColour_1 varchar(10) = '#fc0704' 
    DECLARE @ThemeSecondaryThemeColour_1 varchar(10) = '#d7d3ca' 
	DECLARE @ThemeNavBarColourColour_1 varchar(10) = '#fc0704'
    
	-- *******************
    -- saxton vehicle sales  - customer 2
	-- *******************
    DECLARE @BusinessName_2 varchar(100) = 'Saxton Vehicle Sales'
	DECLARE @CustomerIndexName_2 varchar(100) = 'saxton-vehicle-sales'
	DECLARE @SearchIndexId_2 uniqueidentifier = 'f033ce72-9d92-4dea-b6c5-f0ee57fa3ccc' 
	DECLARE @ThemeId_2 uniqueidentifier = '371a53f5-d462-4fcb-a547-1c025e82f278' 
    DECLARE @ThemeLogoURL_2 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/customer-images/saxton-vehicle-sales-test-image.jpg' 
	DECLARE @ThemeMissingImageURL_2 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/customer-images/saxton-vehicle-sales-test-image.jpg' 
    DECLARE @ThemePrimaryThemeColour_2 varchar(10) = '#3ebcfd'
    DECLARE @ThemeSecondaryThemeColour_2 varchar(10) = '#065265'
	DECLARE @ThemeNavBarColourColour_2 varchar(10) = '#3ebcfd'
    
	-- ******************
    -- wearside autoparc  - customer 3
	-- ******************    
    DECLARE @BusinessName_3 varchar(100) = 'Wearside Autoparc'
	DECLARE @CustomerIndexName_3 varchar(100) = 'wearside-autoparc-vehicles'
	DECLARE @SearchIndexId_3 uniqueidentifier = '9aee3213-da8a-4d44-a1c8-fb6402ac96bc'
	DECLARE @ThemeId_3 uniqueidentifier = '7395eee2-2bb0-41b5-ae48-426aee784eb7' 
    DECLARE @ThemeLogoURL_3 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/customer-images/wearside-autoparc-test-image.jpg'
	DECLARE @ThemeMissingImageURL_3 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/customer-images/wearside-autoparc-test-image.jpg' 
    DECLARE @ThemePrimaryThemeColour_3 varchar(10) = '#004C00'
    DECLARE @ThemeSecondaryThemeColour_3 varchar(10) = '#004C00'
	DECLARE @ThemeNavBarColourColour_3 varchar(10) = '#bdd29a'
	
	-- ******************
    -- jgilmartin motors  - customer 4
	-- ******************    
    DECLARE @BusinessName_4 varchar(100) = 'JGilmartin Motors'
	DECLARE @CustomerIndexName_4 varchar(100) = 'jgilmartin-motors-vehicles'
	DECLARE @SearchIndexId_4 uniqueidentifier = '4cdd1c8a-30f4-4bc7-8de4-3f01ca7bc97f'
	DECLARE @ThemeId_4 uniqueidentifier = '3f62e01c-4ccf-40bb-9bfb-50372d18c977' 
    DECLARE @ThemeLogoURL_4 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/customer-images/wearside-autoparc-test-image.jpg'
	DECLARE @ThemeMissingImageURL_4 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/customer-images/wearside-autoparc-test-image.jpg' 
    DECLARE @ThemePrimaryThemeColour_4 varchar(10) = '#71538f'
    DECLARE @ThemeSecondaryThemeColour_4 varchar(10) = '#3e056e'
	DECLARE @ThemeNavBarColourColour_4 varchar(10) = '#7e3cb5'
	
	-- ******************
    -- dpenaluna motors  - customer 5
	-- ******************    
    DECLARE @BusinessName_5 varchar(100) = 'Dpenaluna Motors'
	DECLARE @CustomerIndexName_5 varchar(100) = 'dpenaluna-motors-vehicles'
	DECLARE @SearchIndexId_5 uniqueidentifier = 'e30f4cb5-dbca-450f-a325-8e9bf2ba232b'
	DECLARE @ThemeId_5 uniqueidentifier = '5b0f357e-4fbf-466c-9f2b-db93fd1fd5ec' 
    DECLARE @ThemeLogoURL_5 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg' 
	DECLARE @ThemeMissingImageURL_5 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/customer-images/wearside-autoparc-test-image.jpg' 
    DECLARE @ThemePrimaryThemeColour_5 varchar(10) = '#d4a420'
    DECLARE @ThemeSecondaryThemeColour_5 varchar(10) = '#6e3405'
	DECLARE @ThemeNavBarColourColour_5 varchar(10) = '#b5793c'   
   
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
	 DECLARE @FeedUsername_1 varchar(100) = 'Marshall_FTP_1'
     DECLARE @FeedPasswordHash_1 varchar(255) = 'xh6NPbvqmDhH6E2vK3mJ'

	 DECLARE @FeedSearchIndexId_2 uniqueidentifier = @SearchIndexId_2
	 DECLARE @FeedId_2 uniqueidentifier = '9875c5ef-c652-47fd-be7d-6eb9cc74723a'
	 DECLARE @FeedUsername_2 varchar(100) = 'Saxton_FTP_2'
     DECLARE @FeedPasswordHash_2 varchar(255) = 'PPk57p7jPXbVQBAK'

	 DECLARE @FeedSearchIndexId_3 uniqueidentifier = @SearchIndexId_3
	 DECLARE @FeedId_3 uniqueidentifier = '2465c76a-ec51-4d3c-9695-834312f154df'
	 DECLARE @FeedUsername_3 varchar(100) = 'Wearside_FTP_3'
     DECLARE @FeedPasswordHash_3 varchar(255) = '3Hcprz6vvK2nMd4qd4HmP'

	 DECLARE @FeedSearchIndexId_4 uniqueidentifier = @SearchIndexId_4
	 DECLARE @FeedId_4 uniqueidentifier = '60007c73-2b76-4239-9dab-9668d90b10ea'
	 DECLARE @FeedUsername_4 varchar(100) = 'JGMotors_FTP_4'
     DECLARE @FeedPasswordHash_4 varchar(255) = 'z4rvchYARvqZEbGU'

	 DECLARE @FeedSearchIndexId_5 uniqueidentifier = @SearchIndexId_5
	 DECLARE @FeedId_5 uniqueidentifier = '33d6c882-0e8b-4c6a-af27-08b4d097d132'
	 DECLARE @FeedUsername_5 varchar(100) = 'DPMotors_FTP_5'
     DECLARE @FeedPasswordHash_5 varchar(255) = 'XZJR@SUPkz2367KzD'

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

TRUNCATE TABLE [insights].[SearchInsightsData]
TRUNCATE TABLE [insights].[SearchIndexRequestLog]

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
			@CustomerId_Marshall_Cars,
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
			@CustomerId_Marshall_Cars,
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
			@CustomerId_Saxton_Vehicle_Data,
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
			@CustomerId_Saxton_Vehicle_Data,
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
			@CustomerId_Wearside_Autoparc,
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
			@CustomerId_Wearside_Autoparc,
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
	PRINT 'Inserting Test Customer 4 - '
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
			@CustomerId_JGilmartin_Motors,
			@BusinessName_4,
			@Now,
			NULL
		) 
	
	PRINT '********************************'
	PRINT 'Inserting Search Index for Test Customer 4 - '
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
			@SearchIndexId_4,
			@SearchInstanceId,
			@CustomerIndexName_4,
			@BusinessName_4,
			@CustomerId_JGilmartin_Motors,
			@Now,
			@SkuIdFree
		) 
	
	PRINT '********************************'
	PRINT 'Search Index Keys for Test Customer 4 - '
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
			@SearchIndexId_4,
			@BusinessName_4 + ' test key',
			@SearchInstanceKeyId_QueryKey,
			@Now
		) 
	
	PRINT '********************************'
	PRINT 'Inserting Feed Entry for Test Customer 4 - '
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
			@SearchIndexId_4,
			@DataFormat,
			@Now,
			null,
			1
		) 

	PRINT '********************************'
	PRINT 'Inserting Test Customer 5 - '
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
			@CustomerId_DPMotors_Motors,
			@BusinessName_5,
			@Now,
			NULL
		) 
	
	PRINT '********************************'
	PRINT 'Inserting Search Index for Test Customer 5 - '
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
			@SearchIndexId_5,
			@SearchInstanceId,
			@CustomerIndexName_5,
			@BusinessName_5,
			@CustomerId_DPMotors_Motors,
			@Now,
			@SkuIdFree
		) 
	
	PRINT '********************************'
	PRINT 'Search Index Keys for Test Customer 5 - '
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
			@SearchIndexId_5,
			@BusinessName_5 + ' test key',
			@SearchInstanceKeyId_QueryKey,
			@Now
		) 
	
	PRINT '********************************'
	PRINT 'Inserting Feed Entry for Test Customer 5 - '
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
			@SearchIndexId_5,
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

-- *******************************
-- marshall-cars - Local host Interface
-- *******************************
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
		@MarshallCarsEndpoint,
        @InterfaceType,
        @InterfaceLogoURL,
		@IntefaceBannerStyle,
		@Now,
		NULL,
		1
    ) 

-- *********************************
-- saxton vehicle data - Azure Demo Environment
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
		@SaxtonVehicleSalesEndpoint,
        @InterfaceType,
        @InterfaceLogoURL,
		@IntefaceBannerStyle,
		@Now,
		NULL,
		1
    )

-- *********************************
-- wearside autoparc data - Azure Demo Environment
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
		@WearsideAutoparcEndpoint,
        @InterfaceType,
        @InterfaceLogoURL,
		@IntefaceBannerStyle,
		@Now,
		NULL,
		1
    )
	
-- *********************************
-- JGilmartin Motors data - Azure Demo Environment
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
        @SearchIndexId_4,
		@JGilmartinMotorsEndpoint,
        @InterfaceType,
        @InterfaceLogoURL,
		@IntefaceBannerStyle,
		@Now,
		NULL,
		1
    )

-- *********************************
-- DPenaluna Motors data - Azure Demo Environment
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
        @SearchIndexId_5,
		@DPMotorsEndpoint,
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
	PRINT 'Customer 1 Theme Details'
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
        @CustomerId_Marshall_Cars,
        @SearchIndexId_1,
		@Now,
		NULL
	)

	PRINT '********************************'
	PRINT 'Customer 2 Theme Details'
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
        @CustomerId_Saxton_Vehicle_Data,
        @SearchIndexId_2,
		@Now,
		NULL
	)

	PRINT '********************************'
	PRINT 'Customer 3 Theme Details'
	PRINT '********************************'

	INSERT INTO [dbo].[Themes]
    (
		[ThemeId]
        ,[PrimaryHexColour]
        ,[SecondaryHexColour]
        ,[NavBarHexColour]
		,[MissingImageURL]
        ,[LogoURL]
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
        @CustomerId_Wearside_Autoparc,
        @SearchIndexId_3,
		@Now,
		NULL
	)	

	PRINT '********************************'
	PRINT 'Customer 4 Theme Details'
	PRINT '********************************'

	INSERT INTO [dbo].[Themes]
    (
		[ThemeId]
        ,[PrimaryHexColour]
        ,[SecondaryHexColour]
        ,[NavBarHexColour]
		,[MissingImageURL]
        ,[LogoURL]
        ,[CustomerId]
        ,[SearchIndexId]
		,[CreatedDate]
		,[ModifiedDate]
	)
    VALUES
    (
		@ThemeId_4,
        @ThemePrimaryThemeColour_4,
        @ThemeSecondaryThemeColour_4,
        @ThemeNavBarColourColour_4,
		@ThemeLogoURL_4,
		@ThemeMissingImageURL_4,
        @CustomerId_JGilmartin_Motors,
        @SearchIndexId_4,
		@Now,
		NULL
	)
	
	PRINT '********************************'
	PRINT 'Customer 5 Theme Details'
	PRINT '********************************'

	INSERT INTO [dbo].[Themes]
    (
		[ThemeId]
        ,[PrimaryHexColour]
        ,[SecondaryHexColour]
        ,[NavBarHexColour]
		,[MissingImageURL]
        ,[LogoURL]
        ,[CustomerId]
        ,[SearchIndexId]
		,[CreatedDate]
		,[ModifiedDate]
	)
    VALUES
    (
		@ThemeId_5,
        @ThemePrimaryThemeColour_5,
        @ThemeSecondaryThemeColour_5,
        @ThemeNavBarColourColour_5,
		@ThemeLogoURL_5,
		@ThemeMissingImageURL_5,
        @CustomerId_DPMotors_Motors,
        @SearchIndexId_5,
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

	PRINT '*************************************************'
	PRINT 'Search Index 4 FeedCredentials - additional index'
	PRINT '*************************************************'

	INSERT INTO [dbo].[FeedCredentials]
			   ([Id]
			   ,[SearchIndexId]
			   ,[Username]
			   ,[PasswordHash]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (@FeedId_4
			   ,@FeedSearchIndexId_4
			   ,@FeedUsername_4
			   ,@FeedPasswordHash_4
			   ,@Now
			   ,null)

	PRINT '*************************************************'
	PRINT 'Search Index 5 FeedCredentials - additional index'
	PRINT '*************************************************'

	INSERT INTO [dbo].[FeedCredentials]
			   ([Id]
			   ,[SearchIndexId]
			   ,[Username]
			   ,[PasswordHash]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (@FeedId_5
			   ,@FeedSearchIndexId_5
			   ,@FeedUsername_5
			   ,@FeedPasswordHash_5
			   ,@Now
			   ,null)

	PRINT '******************************************'
	PRINT 'Setup Marshall Cars Configuration Mappings'	
	PRINT '******************************************'

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
	PRINT 'Setup Saxton Vehicle Sales Configuration Mappings'	
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
	PRINT 'Setup Wearside Autoparc Configuration Mappings'	
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


	PRINT '**********************************************'
	PRINT 'Setup JGilmartin Motors Configuration Mappings'	
	PRINT '**********************************************'

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (@SearchConfigurationMapping_4
			   ,'true'
			   ,@SearchConfigurationOption_EnableAutoComplete_Id
			   ,@SearchIndexId_4
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
			   ,@SearchIndexId_4
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
			   ,@SearchIndexId_4
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
			   ,@SearchIndexId_4
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
			   ,@SearchIndexId_4
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
			   ,@SearchIndexId_4
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
			   ,@SearchIndexId_4
			   ,@Now
			   ,null)

	PRINT '**********************************************'
	PRINT 'Setup DPenaluna Motors Configuration Mappings'	
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
			   ,@SearchIndexId_5
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
			   ,@SearchIndexId_5
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
			   ,@SearchIndexId_5
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
			   ,@SearchIndexId_5
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
			   ,@SearchIndexId_5
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
			   ,@SearchIndexId_5
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
			   ,@SearchIndexId_5
			   ,@Now
			   ,null)

	PRINT '********************************'
	PRINT 'Dummy Data Script - Complete'
	PRINT '********************************'
END
/***************************************************************************************
 Dummy Data - Script End
 ***************************************************************************************/