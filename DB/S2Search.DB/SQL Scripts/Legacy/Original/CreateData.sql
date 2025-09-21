use CustomerResourceStore
go
    /***************************************************************************************
     Static Variables
     ----------------------------------------------------------------------------------------
     These do not change
     ***************************************************************************************/
    declare @Now datetime = GETUTCDATE() 
    declare @Subscription_ResourceGroupsQuota int = 980 
    declare @ResourceGroup_ResourceQuota int = 800

    /***************************************************************************************
     Dummy Data - Script Start
     ***************************************************************************************/
    declare @InsertDummyData bit = 1 --NOTE: This will not link to a search resource in Azure, it is purely for setting up a mock instance with the expected data structure
    IF @InsertDummyData = 1 BEGIN PRINT 'Dummy Data Script - Started' --SearchIndex variables
    declare @CustomerId uniqueidentifier = 'afeb217b-813a-4b9c-82ec-e0221d5e95b1'
    declare @SearchInstanceId uniqueidentifier = '97032266-c1c0-4278-8816-053bbc3a1036'
    declare @ResourceGroup varchar(255) = 'Demo-JG-UKS-AzureSearch'
    declare @ServiceLocation varchar(50) = 'UK South'
    declare @SearchInstanceEndpoint varchar(250) = 'https://demo-dev-jg-uks-1.search.windows.net'
    declare @AdminKey varchar(255) = 'CD52CDDD61C8D031FD2AC1B8AD565205'
    declare @SecondaryKey varchar(255) = '24B1ADDFC3A52A68DC3F3E63542C8F1B'
    declare @QueryKey varchar(255) = '03F111875473D3CE6F5B70C1333EDC96'
    declare @Replicas int = 1
    declare @Partitions int = 1
    declare @IsShared bit = 1
    declare @SearchInstanceName varchar(255) = 'demo-dev-jg-uks-1'
    declare @SearchIndexName varchar(255) = 'vehicles-' + LEFT(CONVERT(varchar(255), NEWID()), 8) --Feeds
    declare @FeedType varchar(20) = 'FTPS'
    declare @FeedCron varchar(50) = '5 * * * *'
	declare @DataFormat varchar(50) = 'DMS14'
    
    --NotificationRules
    declare @NotifyTransmitType varchar(255) = 'email'
    declare @NotifyRecipients varchar(255) = 'notify@square2softwaredevelopment.co.uk'
    declare @NotifyTrigger varchar(255) = 'Feed_Success'
    
    --SearchInterfaces
    declare @InterfaceType varchar(50) = 'API_Consumption'
    declare @InterfaceLogoURL varchar(255) = NULL
    declare @IntefaceBannerStyle varchar(255) = NULL
    
    --Synonyms
	declare @SynonymId_1 uniqueidentifier = '36baeafb-a843-4eb1-a2a9-4457d2eabe0d'
    declare @KeyWord_1 varchar(50) = 'BMW'
    declare @SolrFormat_1 varchar(250) = 'beema, bimma => BMW'

	declare @SynonymId_2 uniqueidentifier = '08719d6f-4e7f-4c3b-9b9b-b83100441012'
	declare @KeyWord_2 varchar(50) = 'volkswagen'
    declare @SolrFormat_2 varchar(250) = 'VW => Volkswagen'

    --Generic Synonyms
    declare @GenericSynonymCategory varchar(50) = 'vehicles'
    declare @GenericSynonymId_1 uniqueidentifier = '45AD8A77-AF90-441F-8946-BAB6DF07B3A0'
    declare @GenericSolrFormat_1 varchar(250) = 'beema, bimma => BMW'

	declare @GenericSynonymId_2 uniqueidentifier = 'A2BEF053-6787-4679-AB9F-0686E54313B6'
    declare @GenericSolrFormat_2 varchar(250) = 'VW => Volkswagen'

    --SearchInstance Keys
    declare @SearchInstanceKeyId_AdminKey uniqueidentifier = '317e8b8a-c274-499c-a4fb-bacdf8d25e81'
    declare @SearchInstanceKeyId_SecondaryAdminKey uniqueidentifier = '3e0217e5-38be-4616-a81d-c1693c83aa5c'
    declare @SearchInstanceKeyId_QueryKey uniqueidentifier = 'fea6648a-f781-4ec0-b8f6-ecf26eb820ac'
    
	-- **************
    -- Configuration
    -- **************

	-- Configurations Option 1 - Enable Auto Complete
	declare @SearchConfigurationOption_EnableAutoComplete_Id uniqueidentifier = '4d833bb7-c52b-49ed-b42d-84a9814b9274'	
	declare @SearchConfigurationOption_EnableAutoComplete_Key varchar(150) = 'EnableAutoComplete'
	declare @SearchConfigurationOption_EnableAutoComplete_FriendlyName varchar(500) = 'Enable Auto Complete'
	declare @SearchConfigurationOption_EnableAutoComplete_Description varchar(MAX) = 'if true, this will configure the search bar to include auto complete suggestions based on the users text inputs'

	-- Configurations Option 2 - Hide Icon Vehicle Counts
	declare @SearchConfigurationOption_HideIconVehicleCounts_Id uniqueidentifier = 'd4d0978e-980c-4237-be79-f2e0ffc0ae06'	
	declare @SearchConfigurationOption_HideIconVehicleCounts_Key varchar(150) = 'HideIconVehicleCounts'
	declare @SearchConfigurationOption_HideIconVehicleCounts_FriendlyName varchar(500) = 'Hide Icon Vehicle Counts'
	declare @SearchConfigurationOption_HideIconVehicleCounts_Description varchar(MAX) = 'if true, this will hide the icon vehicle counts on the top nav bar'

	--Search Configuration Mappings values
	declare @SearchConfigurationMapping_1 uniqueidentifier = '4350fbfb-5b7d-4162-92aa-2a3ec39a4a5f'
	declare @SearchConfigurationMapping_2 uniqueidentifier = '0d4cb168-5bb9-4e4d-8a18-d39afa249eb6'
	declare @SearchConfigurationMapping_3 uniqueidentifier = 'c0fd3d12-294c-41b0-b3ae-ae82cb6b5ca1'
	declare @SearchConfigurationMapping_4 uniqueidentifier = '74a225fd-17ab-421a-a69c-662c2f9a9195'

	-- SearchIndexId is set as S2 Demo
    declare @SearchConfigurationID_Bool uniqueidentifier = '9ac59cb9-ce37-4209-95c5-3c5283922ff3'
	declare @SearchConfigurationID_Int uniqueidentifier = '4c8d4535-9824-4a9f-9791-8f1cf2ec280e'
	declare @SearchConfigurationID_String uniqueidentifier = '679eefca-e1d5-4718-8d3c-41e270b8de1c'
	declare @SearchConfigurationID_Decimal uniqueidentifier = 'd3d1488d-3c4b-4100-80e7-8db70f756957'
	declare @SearchConfigurationID_Array uniqueidentifier = '5d8f0db9-d691-4ebf-ba1e-e38c2c514303'

    -- ***********
    -- customers
    -- ***********

	-- Azure B2C Test User ID Guid -> 37a0eb6c-fd38-4b11-9486-e61ed6745953
	-- Test User -> Malcom Dawkins
	declare @Azure_B2C_User_Malcom_Dawkins uniqueidentifier = '37a0eb6c-fd38-4b11-9486-e61ed6745953'

	-- ******************
    -- marshall-cars - customer 1
	-- ******************
    declare @CustomerId_1 uniqueidentifier = '1fd1a443-474d-40be-b2ee-a80c2323e4d8'
    declare @BusinessName_1 varchar(100) = 'Marshall Cars' 
	declare @CustomerIndexName_1 varchar(100) = 'marshall-cars-vehicles'
	declare @SearchIndexId_1 uniqueidentifier = '6fb3c6d1-f480-47b5-9934-02e611aefd02' 
	declare @ThemeId_1 uniqueidentifier = 'd012613b-067d-463b-87ba-6150447b7192' 
    declare @ThemeLogoURL_1 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg' 
    declare @ThemePrimaryThemeColour_1 varchar(10) = '#B20000' 
    declare @ThemeSecondaryThemeColour_1 varchar(10) = '#500000' 
	declare @ThemeNavBarColourColour_1 varchar(10) = '#6dbf15'    
    
	-- *******************
    -- saxton vehicle data  - customer 2
	-- *******************
    declare @CustomerId_2 uniqueidentifier = '1d5954c9-5084-42b3-8891-4e428ab44170'
    declare @BusinessName_2 varchar(100) = 'Saxton Vehicle Sales'
	declare @CustomerIndexName_2 varchar(100) = 'saxton-vehicle-sales'
	declare @SearchIndexId_2 uniqueidentifier = 'f033ce72-9d92-4dea-b6c5-f0ee57fa3ccc' 
	declare @ThemeId_2 uniqueidentifier = '371a53f5-d462-4fcb-a547-1c025e82f278' 
    declare @ThemeLogoURL_2 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg' 
    declare @ThemePrimaryThemeColour_2 varchar(10) = '#0000CC'
    declare @ThemeSecondaryThemeColour_2 varchar(10) = '#000066'
	declare @ThemeNavBarColourColour_2 varchar(10) = '#5b77a6'    
    
	-- ******************
    -- wearside autoparc  - customer 3
	-- ******************
    declare @CustomerId_3 uniqueidentifier = 'ff64f7c0-18e9-4326-81d3-6ab39929f0c7'
    declare @BusinessName_3 varchar(100) = 'Westside Autoparc'
	declare @CustomerIndexName_3 varchar(100) = 'westside-autoparc-vehicles'
	declare @SearchIndexId_3 uniqueidentifier = '9aee3213-da8a-4d44-a1c8-fb6402ac96bc'
	declare @ThemeId_3 uniqueidentifier = '7395eee2-2bb0-41b5-ae48-426aee784eb7' 
    declare @ThemeLogoURL_3 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg'
    declare @ThemePrimaryThemeColour_3 varchar(10) = '#008000'
    declare @ThemeSecondaryThemeColour_3 varchar(10) = '#004C00'
	declare @ThemeNavBarColourColour_3 varchar(10) = '#d932bd'    

	-- ********************************************
	-- ** S2-Demo Endpoints
	-- ********************************************

	declare @LocalDevEndpoint varchar(100) = 'localhost:4280' 
	declare @AzureDevEndpoint varchar(100) = 's2searchui.azurewebsites.net' 

	-- ******************
	-- S2 Demo - customer 4 ** This is the test user that is linked to the test Azure B2C user **
	-- @CustomerId_4 - maps to the object ID for the Azure B2C test user Malcom Dawkins *********
	-- ******************
    --declare @CustomerId_6 uniqueidentifier = ##GUID##
    declare @BusinessName_4 varchar(100) = 'S2 Demo'
	declare @CustomerIndexName_4 varchar(100) = 's2-demo'
	declare @SearchIndexId_4 uniqueidentifier = '06069481-4f30-4fa6-8776-6893a60bd73c'
	declare @ThemeId_4 uniqueidentifier = '220bfc7a-f06f-48f2-bc19-d02fcb5325bb' 
    declare @ThemeLogoURL_4 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg'
    declare @ThemePrimaryThemeColour_4 varchar(10) = '#f17edc'
    declare @ThemeSecondaryThemeColour_4 varchar(10) = '#9c5bc1'
	declare @ThemeNavBarColourColour_4 varchar(10) = '#9ec45c' 

	-- ******************
	-- s2-demo-2 - customer 4 ** this is a secondary search index used by the demo customer **
	-- maps to the object ID for the Azure B2C test user Malcom Dawkins *********
	-- ******************
    --declare @CustomerId_5 uniqueidentifier = ##GUID##
    declare @BusinessName_5 varchar(100) = 'S2 Demo 2'
	declare @CustomerIndexName_5 varchar(100) = 's2-demo-2'
	declare @SearchIndexId_5 uniqueidentifier = 'eccce591-c720-4c4f-88b6-a55945c2b16a'
	declare @ThemeId_5 uniqueidentifier = '57716416-7db8-401c-839e-1fad64f4941b' 
    declare @ThemeLogoURL_5 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg'
    declare @ThemePrimaryThemeColour_5 varchar(10) = '#bbd6aa'
    declare @ThemeSecondaryThemeColour_5 varchar(10) = '#1b0ace'
	declare @ThemeNavBarColourColour_5 varchar(10) = '#cc04c6'

	-- ******************
	-- s2-demo-3 - customer 5 ** this is a third test index used by the demo customer **
	-- maps to the object ID for the Azure B2C test user Malcom Dawkins *********
	-- ******************
    declare @CustomerId_4 uniqueidentifier = @Azure_B2C_User_Malcom_Dawkins
    declare @BusinessName_6 varchar(100) = 'S2 Demo 3'
	declare @CustomerIndexName_6 varchar(100) = 's2-demo-3'
	declare @SearchIndexId_6 uniqueidentifier = 'a65ce2fa-ce80-4399-9ec5-01ef2d3a5a36'
	declare @ThemeId_6 uniqueidentifier = '9329c137-e340-49ca-9b46-539a99c41e45' 
    declare @ThemeLogoURL_6 varchar(1000) = 'https://dpdevstore.blob.core.windows.net/temp/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg'
    declare @ThemePrimaryThemeColour_6 varchar(10) = '#1c4215'
    declare @ThemeSecondaryThemeColour_6 varchar(10) = '#4f1f30'
	declare @ThemeNavBarColourColour_6 varchar(10) = '#c6e384' 

   
    /***************************************************************************************
     Required Variables
     ----------------------------------------------------------------------------------------
     You will need to set these variables for the script to run successfully.
     See the README for instructions on how to set up a ServicePrinciple in Azure
     ***************************************************************************************/
    declare @SubscriptionId uniqueidentifier = 'f8cff945-b5e5-462a-9786-d69bd7a0eb34'
    declare @SubscriptionName varchar(50) = 'S2-Pay-As-You-Go'
    declare @ServicePrinciple_Name varchar(50) = 'TestProvision'
    declare @ServicePrinciple_ClientId uniqueidentifier = '5ba3c688-71f9-474a-952f-c7b527a29f65' 
    declare @ServicePrinciple_TenantId uniqueidentifier = '0694e993-9911-4269-b7e2-bfd29ca197c6' 
    declare @ServicePrinciple_ClientKeyName varchar(50) = 'test' 
    declare @ServicePrinciple_ClientKeySecret varchar(100) = '64ZZWyvGfVC~A1e.YF9IjQF.kXx-3hfOn4' 
    declare @ServicePrinciple_ClientKeyExpiryDate datetime = '2021-11-11 00:00' 
    declare @AzurePricingTier varchar(50) = 'Free'

     /***************************************************************************************
     Customer Pricing Tiers
     ***************************************************************************************/
     declare @SkuIdFree varchar(50) = 'FREE'
     declare @SkuIdFreeName varchar(100) = 'S2 Free Tier'
     declare @SkuIdFreeDescription varchar(255) = 'TBD'
     declare @SkuIdFreeEffectiveFrom datetime = '2021-03-24 00:00:00.000'

     declare @SkuIdFreeTrial varchar(50) = 'FREETRIALMARCH'
     declare @SkuIdFreeTrialName varchar(100) = 'S2 Free Trial March 2021'
     declare @SkuIdFreeTrialDescription varchar(255) = 'TBD'
     declare @SkuIdFreeTrialEffectiveFrom datetime = '2021-03-24 00:00:00.000'
     declare @SkuIdFreeTrialEffectiveTo datetime = '2021-03-31 23:59:59.000'

     /***************************************************************************************
     Feed Credentials
     ***************************************************************************************/
	 -- SearchIndexId is set as S2 Demo
     declare @FeedSearchIndexId_1 uniqueidentifier = @SearchIndexId_4
     declare @FeedId_1 uniqueidentifier = '7bcc246b-c8e0-4d91-bb8a-ec5f8c7b3230'
	 declare @FeedUsername_1 varchar(100) = 'S2DemoFTP'
     declare @FeedPasswordHash_1 varchar(255) = 'xh6NPbvqmDhH6E2vK3mJ'

	 declare @FeedSearchIndexId_2 uniqueidentifier = @SearchIndexId_5
	 declare @FeedId_2 uniqueidentifier = '9875c5ef-c652-47fd-be7d-6eb9cc74723a'
	 declare @FeedUsername_2 varchar(100) = 'S2DemoFTP_2'
     declare @FeedPasswordHash_2 varchar(255) = 'PPk57p7jPXbVQBAK'

	 declare @FeedSearchIndexId_3 uniqueidentifier = @SearchIndexId_6
	 declare @FeedId_3 uniqueidentifier = '2465c76a-ec51-4d3c-9695-834312f154df'
	 declare @FeedUsername_3 varchar(100) = 'S2DemoFTP_3'
     declare @FeedPasswordHash_3 varchar(255) = '3Hcprz6vvK2nMd4qd4HmP'

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
        ,[CreatedDate]
		,[ModifiedDate])
    VALUES
        (@SearchConfigurationOption_EnableAutoComplete_Id
        ,@SearchConfigurationOption_EnableAutoComplete_Key
        ,@SearchConfigurationOption_EnableAutoComplete_FriendlyName
        ,@SearchConfigurationOption_EnableAutoComplete_Description
		,@SearchConfigurationID_Bool
        ,@Now
		,null)

INSERT INTO [dbo].[SearchConfigurationOptions]
        ([SeachConfigurationOptionId]
        ,[Key]
        ,[FriendlyName]
        ,[Description]
		,[SearchConfigurationDataTypeId]
        ,[CreatedDate]
		,[ModifiedDate])
    VALUES
        (@SearchConfigurationOption_HideIconVehicleCounts_Id
        ,@SearchConfigurationOption_HideIconVehicleCounts_Key
        ,@SearchConfigurationOption_HideIconVehicleCounts_FriendlyName
        ,@SearchConfigurationOption_HideIconVehicleCounts_Description
		,@SearchConfigurationID_Bool
        ,@Now
		,null)
	
insert into
    dbo.Subscriptions (SubscriptionId, [Name])
Values
    (@SubscriptionId, @SubscriptionName)
insert into
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
insert into
    dbo.SubscriptionResourceCapacity (
        SubscriptionId,
        PricingTier,
        ResourceType,
        Quota,
        Available,
        Used,
        ModifiedDate
    )
values
    (
        @SubscriptionId,
        @AzurePricingTier,
        'Azure Cognitive Search',
        1,
        1,
        0,
        @Now
    )
insert into
    dbo.ServicePrinciples (
        ClientId,
        SubscriptionId,
        [Name],
        TenantId
    )
values
    (
        @ServicePrinciple_ClientId,
        @SubscriptionId,
        @ServicePrinciple_Name,
        @ServicePrinciple_TenantId
    )
insert into
    dbo.ClientKeys (ClientId, [Name], [Value], ExpiryDate)
VALUES
    (
        @ServicePrinciple_ClientId,
        @ServicePrinciple_ClientKeyName,
        @ServicePrinciple_ClientKeySecret,
        @ServicePrinciple_ClientKeyExpiryDate
    )

insert into 
    dbo.CustomerPricingTiers (SkuId, [Name], [Description], EffectiveFromDate, EffectiveToDate)
values
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

insert into
    dbo.CustomerPricing (SkuId, Price, EffectiveFromDate, EffectiveToDate)
values
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

insert into
    dbo.ResourceGroups (ResourceGroup, SubscriptionId)
Values
    (@ResourceGroup, @SubscriptionId) PRINT 'Inserting Resource Group Capacity'
insert into
    dbo.ResourceGroupsCapacity (
        ResourceGroup,
        ResourcesQuota,
        ResourcesUsed,
        ResourcesAvailable,
        ModifiedDate
    )
values
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

insert into
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
Values
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

insert into
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
Values
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
insert into
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
Values
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
insert into
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
Values
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
	
	PRINT 'Inserting Test Customer 1 - '

insert into
    dbo.Customers (
        [CustomerId],
        [BusinessName],
		[CreatedDate],
		[ModifiedDate]
    )
Values
    (
        @CustomerId_1,
        @BusinessName_1,
		@Now,
		NULL
    )
	
	PRINT '********************************'
	PRINT 'Inserting Search Index for Test Customer 1 - '
	PRINT '********************************'

insert into
    dbo.SearchIndex (
        SearchIndexId,
        SearchInstanceId,
        IndexName,
        FriendlyName,
        CustomerId,
        CreatedDate,
        PricingSkuId
    )
Values
    (
        @SearchIndexId_1,
        @SearchInstanceId,
        @CustomerIndexName_1,
        @BusinessName_1,
        @CustomerId_1,
        @Now,
        @SkuIdFree
    ) 
	
	PRINT '********************************'
	PRINT 'Search Index Keys for Test Customer 1 - '
	PRINT '********************************'

insert into
    dbo.SearchIndexKeys (
        [SearchIndexId],
        [Name],
        [SearchInstanceKeyId],
        [CreatedDate]
    )
values
    (
        @SearchIndexId_1,
        @BusinessName_1 + ' test key',
		@SearchInstanceKeyId_QueryKey,
        @Now
    )
	
	PRINT '********************************'
	PRINT 'Inserting Feed Entry for Test Customer 1 - '
	PRINT '********************************'

insert into
    dbo.Feeds (
      [FeedType]
      ,[FeedScheduleCron]
      ,[SearchIndexId]
      ,[DataFormat]
      ,[CreatedDate]
      ,[SupersededDate]
      ,[IsLatest]
    )
values
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

insert into
    dbo.Customers (
        [CustomerId],
        [BusinessName],
		[CreatedDate],
		[ModifiedDate]
    )
Values
    (
        @CustomerId_2,
        @BusinessName_2,
		@Now,
		NULL
    ) 
	
	PRINT '********************************'
	PRINT 'Inserting Search Index for Test Customer 2 - '
	PRINT '********************************'

insert into
    dbo.SearchIndex (
        SearchIndexId,
        SearchInstanceId,
        IndexName,
        FriendlyName,
        CustomerId,
        CreatedDate,
        PricingSkuId
    )
Values
    (
        @SearchIndexId_2,
        @SearchInstanceId,
        @CustomerIndexName_2,
        @BusinessName_2,
        @CustomerId_2,
        @Now,
        @SkuIdFree
    ) 
	
	PRINT '********************************'
	PRINT 'Search Index Keys for Test Customer 2 - '
	PRINT '********************************'

insert into
    dbo.SearchIndexKeys (
        [SearchIndexId],
        [Name],
        [SearchInstanceKeyId],
        [CreatedDate]
    )
values
    (
        @SearchIndexId_2,
        @BusinessName_2 + ' test key',
        @SearchInstanceKeyId_QueryKey,
        @Now
    ) 
	
	PRINT '********************************'
	PRINT 'Inserting Feed Entry for Test Customer 2 - '
	PRINT '********************************'

insert into
    dbo.Feeds (
      [FeedType]
      ,[FeedScheduleCron]
      ,[SearchIndexId]
      ,[DataFormat]
      ,[CreatedDate]
      ,[SupersededDate]
      ,[IsLatest]
    )
values
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

insert into
    dbo.Customers (
        [CustomerId],
        [BusinessName],
		[CreatedDate],
		[ModifiedDate]
    )
Values
    (
        @CustomerId_3,
        @BusinessName_3,
		@Now,
		NULL
    ) 
	
	PRINT '********************************'
	PRINT 'Inserting Search Index for Test Customer 3 - '
	PRINT '********************************'

insert into
    dbo.SearchIndex (
        SearchIndexId,
        SearchInstanceId,
        IndexName,
        FriendlyName,
        CustomerId,
        CreatedDate,
        PricingSkuId
    )
Values
    (
        @SearchIndexId_3,
        @SearchInstanceId,
        @CustomerIndexName_3,
        @BusinessName_3,
        @CustomerId_3,
        @Now,
        @SkuIdFree
    ) 
	
	PRINT '********************************'
	PRINT 'Search Index Keys for Test Customer 3 - '
	PRINT '********************************'

insert into
    dbo.SearchIndexKeys (
        [SearchIndexId],
        [Name],
        [SearchInstanceKeyId],
        [CreatedDate]
    )
values
    (
        @SearchIndexId_3,
        @BusinessName_3 + ' test key',
        @SearchInstanceKeyId_QueryKey,
        @Now
    ) 
	
	PRINT '********************************'
	PRINT 'Inserting Feed Entry for Test Customer 3 - '
	PRINT '********************************'

insert into
    dbo.Feeds (
      [FeedType]
      ,[FeedScheduleCron]
      ,[SearchIndexId]
      ,[DataFormat]
      ,[CreatedDate]
      ,[SupersededDate]
      ,[IsLatest]
    )
values
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

insert into
    dbo.Customers (
        [CustomerId],
        [BusinessName],
		[CreatedDate],
		[ModifiedDate]
    )
Values
    (
        @CustomerId_4,
        @BusinessName_4,
		@Now,
		NULL
    ) 
	
	PRINT '********************************'
	PRINT 'Inserting Search Index for Test Customer 4 - '
	PRINT '********************************'

insert into
    dbo.SearchIndex (
        SearchIndexId,
        SearchInstanceId,
        IndexName,
        FriendlyName,
        CustomerId,
        CreatedDate,
        PricingSkuId
    )
Values
    (
        @SearchIndexId_4,
        @SearchInstanceId,
        @CustomerIndexName_4,
        @BusinessName_4,
        @CustomerId_4,
        @Now,
        @SkuIdFree
    )
	
	PRINT '******************************************************************'
	PRINT 'Inserting additional Search Index for Test Customer 5 (s2-demo-2) - '
	PRINT '******************************************************************'

insert into
    dbo.SearchIndex (
        SearchIndexId,
        SearchInstanceId,
        IndexName,
        FriendlyName,
        CustomerId,
        CreatedDate,
        PricingSkuId
    )
Values
    (
        @SearchIndexId_5,
        @SearchInstanceId,
        @CustomerIndexName_5,
        @BusinessName_5,
        @CustomerId_4,
        @Now,
        @SkuIdFree
    ) 

	PRINT '******************************************************************'
	PRINT 'Inserting additional Search Index for Test Customer 6 (s2-demo-3) - '
	PRINT '******************************************************************'

insert into
    dbo.SearchIndex (
        SearchIndexId,
        SearchInstanceId,
        IndexName,
        FriendlyName,
        CustomerId,
        CreatedDate,
        PricingSkuId
    )
Values
    (
        @SearchIndexId_6,
        @SearchInstanceId,
        @CustomerIndexName_6,
        @BusinessName_6,
        @CustomerId_4,
        @Now,
        @SkuIdFree
    ) 
	
	PRINT '********************************'
	PRINT 'Search Index Keys for Test Customer 4 - '
	PRINT '********************************'

insert into
    dbo.SearchIndexKeys (
        [SearchIndexId],
        [Name],
        [SearchInstanceKeyId],
        [CreatedDate]
    )
values
    (
        @SearchIndexId_4,
        @BusinessName_4 + ' test key',
        @SearchInstanceKeyId_QueryKey,
        @Now
    ) 
	
	PRINT '********************************'
	PRINT 'Inserting Feed Entry for Test Customer 4 - '
	PRINT '********************************'

insert into
    dbo.Feeds (
      [FeedType]
      ,[FeedScheduleCron]
      ,[SearchIndexId]
      ,[DataFormat]
      ,[CreatedDate]
      ,[SupersededDate]
      ,[IsLatest]
    )
values
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
	PRINT 'Inserting Feed Entry for S2 Demo 2 - '
	PRINT '********************************'

insert into
    dbo.Feeds (
      [FeedType]
      ,[FeedScheduleCron]
      ,[SearchIndexId]
      ,[DataFormat]
      ,[CreatedDate]
      ,[SupersededDate]
      ,[IsLatest]
    )
values
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
	PRINT 'Inserting Feed Entry for S2 Demo 3 - '
	PRINT '********************************'

insert into
    dbo.Feeds (
      [FeedType]
      ,[FeedScheduleCron]
      ,[SearchIndexId]
      ,[DataFormat]
      ,[CreatedDate]
      ,[SupersededDate]
      ,[IsLatest]
    )
values
    (
        @FeedType,
        @FeedCron,
        @SearchIndexId_6,
		@DataFormat,
        @Now,
        null,
        1
    ) 

	PRINT '********************************'
	PRINT 'Inserting Notification Rule Entry'
	PRINT '********************************'

insert into
    dbo.NotificationRules (
        SearchIndexId,
        TransmitType,
        Recipients,
        [Trigger]
    )
values
    (
        @SearchIndexId_1,
        @NotifyTransmitType,
        @NotifyRecipients,
        @NotifyTrigger
    ) 
	
	PRINT '********************************'
	PRINT 'Inserting Search Interface Entry'
	PRINT '********************************'

-- *******************************
-- s2-demo - Local host Interface
-- *******************************
insert into
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
values
    (
        @SearchIndexId_6,
		@LocalDevEndpoint,
        @InterfaceType,
        @InterfaceLogoURL,
		@IntefaceBannerStyle,
		@Now,
		NULL,
		1
    ) 

-- *********************************
-- s2-demo - Azure Demo Environment
-- *********************************
insert into
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
values
    (
        @SearchIndexId_5,
		@AzureDevEndpoint,
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

insert into
    dbo.[Synonyms] (
		SynonymId,
        SearchIndexId,
        KeyWord,
        SolrFormat
    )
values
    (
		@SynonymId_1,
        @SearchIndexId_1,
        @KeyWord_1,
        @SolrFormat_1
    ) 

insert into
    dbo.[Synonyms] (
		SynonymId,
        SearchIndexId,
        KeyWord,
        SolrFormat
    )
values
    (
		@SynonymId_2,
        @SearchIndexId_1,
        @KeyWord_2,
        @SolrFormat_2
    ) 

    PRINT '********************************'
	PRINT 'Inserting Generic Synonyms'
	PRINT '********************************'

    insert into dbo.GenericSynonyms 
    (
		Id,
        Category,
        SolrFormat
    )
    values
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

insert into [dbo].[Themes]
    (
		[ThemeId]
        ,[PrimaryHexColour]
        ,[SecondaryHexColour]
        ,[NavBarHexColour]
        ,[LogoURL]
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
        @CustomerId_1,
        @SearchIndexId_1,
		@Now,
		NULL
	)

	PRINT '********************************'
	PRINT 'Customer 2 Theme Details'
	PRINT '********************************'

insert into [dbo].[Themes]
    (
		[ThemeId]
        ,[PrimaryHexColour]
        ,[SecondaryHexColour]
        ,[NavBarHexColour]
        ,[LogoURL]
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
        @CustomerId_2,
        @SearchIndexId_2,
		@Now,
		NULL
	)

	PRINT '********************************'
	PRINT 'Customer 3 Theme Details'
	PRINT '********************************'

insert into [dbo].[Themes]
    (
		[ThemeId]
        ,[PrimaryHexColour]
        ,[SecondaryHexColour]
        ,[NavBarHexColour]
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
        @CustomerId_3,
        @SearchIndexId_3,
		@Now,
		NULL
	)	

	PRINT '********************************'
	PRINT 'Customer 4 Theme Details'
	PRINT '********************************'

insert into [dbo].[Themes]
    (
		[ThemeId]
        ,[PrimaryHexColour]
        ,[SecondaryHexColour]
        ,[NavBarHexColour]
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
        @CustomerId_4,
        @SearchIndexId_4,
		@Now,
		NULL
	)
	
	PRINT '*******************************************'
	PRINT 'Customer 5 Theme Details - additional index'
	PRINT '*******************************************'

insert into [dbo].[Themes]
    (
		[ThemeId]
        ,[PrimaryHexColour]
        ,[SecondaryHexColour]
        ,[NavBarHexColour]
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
        @CustomerId_4,
        @SearchIndexId_5,
		@Now,
		NULL
	)

	PRINT '*******************************************'
	PRINT 'Customer 6 Theme Details - additional index'
	PRINT '*******************************************'

insert into [dbo].[Themes]
    (
		[ThemeId]
        ,[PrimaryHexColour]
        ,[SecondaryHexColour]
        ,[NavBarHexColour]
        ,[LogoURL]
        ,[CustomerId]
        ,[SearchIndexId]
		,[CreatedDate]
		,[ModifiedDate]
	)
    VALUES
    (
		@ThemeId_6,
        @ThemePrimaryThemeColour_6,
        @ThemeSecondaryThemeColour_6,
        @ThemeNavBarColourColour_6,
		@ThemeLogoURL_6,
        @CustomerId_4,
        @SearchIndexId_6,
		@Now,
		NULL
	)
	
	PRINT '*******************************************'
	PRINT 'Search Index 4 FeedCredentials'
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
	PRINT 'Search Index 5 FeedCredentials - additional index'
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

	PRINT '*******************************************'
	PRINT 'Search Index 6 FeedCredentials - additional index'
	PRINT '*******************************************'

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
	
	PRINT '********************************************************************************************************'
	PRINT 'Setup Configuration Mappings for Customer 4 - S2 Demo, Customer 5 - S2 Demo-2 and Customer 6 - S2 Demo-3'
	PRINT '********************************************************************************************************'

	INSERT INTO [dbo].[SearchConfigurationMappings]
			   ([SearchConfigurationMappingId]
			   ,[Value]
			   ,[SeachConfigurationOptionId]
			   ,[SearchIndexId]
			   ,[CreatedDate]
			   ,[ModifiedDate])
		 VALUES
			   (@SearchConfigurationMapping_1
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
			   (@SearchConfigurationMapping_2
			   ,'false'
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
			   (@SearchConfigurationMapping_3
			   ,'false'
			   ,@SearchConfigurationOption_HideIconVehicleCounts_Id
			   ,@SearchIndexId_6
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
			   (@SearchConfigurationMapping_4
			   ,'true'
			   ,@SearchConfigurationOption_EnableAutoComplete_Id
			   ,@SearchIndexId_6
			   ,@Now
			   ,null)

	PRINT '********************************'
	PRINT 'Dummy Data Script - Complete'
	PRINT '********************************'
END
/***************************************************************************************
 Dummy Data - Script End
 ***************************************************************************************/