-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Admin].[GetCustomerByID]
	-- Add the parameters for the stored procedure here
	@CustomerId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
	c.[Id],
	c.[BusinessName]
	FROM [dbo].[Customers] c 
	WHERE c.Id = @CustomerId
END
GO
PRINT N'Creating Procedure [Admin].[GetCustomerFull]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Admin].[GetCustomerFull]
	-- Add the parameters for the stored procedure here
	@CustomerId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT c.[Id]
		  ,c.[BusinessName]
	  FROM [dbo].[Customers] c 
	  WHERE c.Id = @CustomerId

	  SELECT [Id]
      ,[CustomerId]
      ,[SearchInstanceId]
      ,[IndexName]
      ,[FriendlyName]
      ,[CreatedDate]
	  FROM [dbo].[SearchIndex] s
	  WHERE s.CustomerId = @CustomerId

END
GO
PRINT N'Creating Procedure [Admin].[GetLatestFeed]...';


GO
CREATE PROCEDURE [Admin].[GetLatestFeed]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

SELECT TOP 1
f.Id,
f.SearchIndexId,
f.FeedType as [Type],
f.FeedScheduleCron as ScheduleCron,
f.CreatedDate,
f.SupersededDate,
f.IsLatest
FROM dbo.Feeds f
WHERE f.SearchIndexId = @SearchIndexId
AND f.IsLatest = 1

END
GO
PRINT N'Creating Procedure [Admin].[GetSearchIndex]...';


GO
CREATE PROCEDURE [Admin].[GetSearchIndex]
(
	@SearchIndexId uniqueidentifier,
	@CustomerId uniqueidentifier
)
AS

BEGIN

SELECT
[search].Id,
[search].CustomerId,
[search].IndexName,
[search].FriendlyName,
[service].RootEndpoint,
[service].PricingTier,
[search].CreatedDate,
[service].Id,
[service].ServiceName,
[service].[Location],
[service].PricingTier,
[service].Replicas,
[service].[Partitions],
[service].IsShared
FROM dbo.SearchIndex [search]
LEFT OUTER JOIN dbo.SearchInstances [service] on [service].Id = search.SearchInstanceId
WHERE search.Id = @SearchIndexId
AND search.CustomerId = @CustomerId

END
GO
PRINT N'Creating Procedure [Admin].[GetSearchIndexByFriendlyName]...';


GO
CREATE PROCEDURE [Admin].[GetSearchIndexByFriendlyName]
(
	@CustomerId uniqueidentifier,
	@FriendlyName varchar(100)
)
AS

BEGIN

SELECT
si.Id,
si.SearchInstanceId,
si.CustomerId,
si.FriendlyName,
si.IndexName
FROM dbo.SearchIndex si
WHERE si.CustomerId = @CustomerId 
AND si.FriendlyName = @FriendlyName

END
GO
PRINT N'Creating Procedure [Admin].[GetSearchIndexFull]...';


GO
CREATE PROCEDURE [Admin].[GetSearchIndexFull]
(
	@SearchIndexId uniqueidentifier,
	@CustomerId uniqueidentifier
)
AS

BEGIN

SELECT
[search].Id,
[search].CustomerId,
[search].IndexName,
[search].FriendlyName,
[service].RootEndpoint,
[service].PricingTier,
[search].CreatedDate,
[service].Id,
[service].ServiceName,
[service].[Location],
[service].PricingTier,
[service].Replicas,
[service].[Partitions],
[service].IsShared
FROM dbo.SearchIndex [search]
LEFT OUTER JOIN dbo.SearchInstances [service] on [service].Id = search.Id
WHERE search.Id = @SearchIndexId
AND search.CustomerId = @CustomerId

--If no results it didnt match on SearchIndexId and CustomerId so override the SearchIndexId so that the other selects do not return a result
IF @@ROWCOUNT = 0
BEGIN
	SET @SearchIndexId = NULL
END

SELECT 
Id,
SearchIndexId,
FeedType as [Type],
FeedScheduleCron as ScheduleCron,
CreatedDate,
SupersededDate,
IsLatest
FROM dbo.Feeds 
WHERE SearchIndexId = @SearchIndexId 
AND IsLatest = 1


SELECT 
Id,
KeyWord as [Key],
SolrFormat
FROM dbo.[Synonyms]
WHERE SearchIndexId = @SearchIndexId 
AND IsLatest = 1

END
GO
PRINT N'Creating Procedure [Admin].[GetSearchIndexQueryCredentialsByCustomerEndpoint]...';


GO
CREATE PROCEDURE [Admin].[GetSearchIndexQueryCredentialsByCustomerEndpoint]
(
	@CustomerEndpoint varchar(250)
)
AS

BEGIN

SELECT
si.Id,
LOWER(si.IndexName) as [SearchIndexName],
i.ServiceName as [SearchInstanceName],
i.[RootEndpoint] as SearchInstanceEndpoint,
ik.ApiKey
FROM dbo.SearchIndex si
INNER JOIN dbo.SearchInstances i on i.Id = si.SearchInstanceId
INNER JOIN dbo.Customers c on si.CustomerId = c.Id
INNER JOIN dbo.SearchInstanceKeys ik on ik.SearchInstanceId = i.Id 
									AND ik.KeyType = 'Query' 
									AND ik.Name = 'Query key' 
									AND ik.IsLatest = 1
WHERE c.CustomerEndpoint = @CustomerEndpoint

END
GO
PRINT N'Creating Procedure [Admin].[GetSearchInsightsByDataCategories]...';


GO
CREATE PROCEDURE [Admin].[GetSearchInsightsByDataCategories]
(
	@SearchIndexId uniqueidentifier,
	@DateFrom datetime,
	@DateTo datetime,
	@DataCategories varchar(1000)
)
AS

BEGIN

SELECT 
d.[DataCategory],
d.[DataPoint],
d.[Date],
d.[Count]
FROM dbo.SearchInsightsData d
CROSS APPLY string_split(@DataCategories, ',') categories
WHERE d.SearchIndexId = @SearchIndexId
AND d.[Date] >= @DateFrom
AND d.[Date] <= @DateTo
AND categories.value = d.DataCategory

END
GO
PRINT N'Creating Procedure [Admin].[GetSearchInsightsSearchCountByDateRange]...';


GO
CREATE PROCEDURE [Admin].[GetSearchInsightsSearchCountByDateRange]
(
	@SearchIndexId uniqueidentifier,
	@DateFrom datetime,
	@DateTo datetime
)
AS

BEGIN

SELECT 
d.[Date],
d.[Count]
FROM dbo.SearchIndexRequestLog d
WHERE d.SearchIndexId = @SearchIndexId
AND d.[Date] BETWEEN @DateFrom AND @DateTo

END
GO
PRINT N'Creating Procedure [Admin].[GetSynonymById]...';


GO
CREATE PROCEDURE [Admin].[GetSynonymById]
(
	@SearchIndexId uniqueidentifier,
	@SynonymId uniqueidentifier
)
AS

BEGIN

SELECT
Id,
SearchIndexId,
KeyWord as [Key],
SolrFormat,
CreatedDate
FROM [dbo].[Synonyms]
WHERE SearchIndexId = @SearchIndexId
AND Id = @SynonymId
AND IsLatest = 1

END
GO
PRINT N'Creating Procedure [Admin].[GetSynonymByKeyWord]...';


GO
CREATE PROCEDURE [Admin].[GetSynonymByKeyWord]
(
	@SearchIndexId uniqueidentifier,
	@KeyWord varchar(30)
)
AS

BEGIN

SELECT
Id,
SearchIndexId,
KeyWord as [Key],
SolrFormat,
CreatedDate
FROM [dbo].[Synonyms]
WHERE SearchIndexId = @SearchIndexId
AND KeyWord = @KeyWord
AND IsLatest = 1

END
GO
PRINT N'Creating Procedure [Admin].[GetSynonyms]...';


GO
CREATE PROCEDURE [Admin].[GetSynonyms]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

SELECT
Id,
SearchIndexId,
KeyWord as [Key],
SolrFormat,
CreatedDate
FROM [dbo].[Synonyms]
WHERE SearchIndexId = @SearchIndexId
AND IsLatest = 1

END
GO
PRINT N'Creating Procedure [Admin].[GetThemeByCustomerId]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Admin].[GetThemeByCustomerId]
	-- Add the parameters for the stored procedure here
	@CustomerId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Id]
      ,[PrimaryHexColour]
      ,[SecondaryHexColour]
      ,[NavBarHexColour]
      ,[LogoURL]
	  ,[MissingImageURL]
      ,[CustomerId]
      ,[SearchIndexId]
      ,[CreatedDate]
      ,[ModifiedDate]
	FROM [dbo].[Themes]
	WHERE [CustomerId] = @CustomerId

END
GO
PRINT N'Creating Procedure [Admin].[GetThemeById]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Admin].[GetThemeById]
	-- Add the parameters for the stored procedure here
	@ThemeId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Id]
      ,[PrimaryHexColour]
      ,[SecondaryHexColour]
      ,[NavBarHexColour]
      ,[LogoURL]
	  ,[MissingImageURL]
      ,[CustomerId]
      ,[SearchIndexId]
      ,[CreatedDate]
      ,[ModifiedDate]
	FROM [dbo].[Themes]
	WHERE [Id] = @ThemeId

END
GO
PRINT N'Creating Procedure [Admin].[GetThemeBySearchIndexId]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Admin].[GetThemeBySearchIndexId]
	-- Add the parameters for the stored procedure here
	@SearchIndexId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Id]
      ,[PrimaryHexColour]
      ,[SecondaryHexColour]
      ,[NavBarHexColour]
      ,[LogoURL]
	  ,[MissingImageURL]
      ,[CustomerId]
      ,[SearchIndexId]
      ,[CreatedDate]
      ,[ModifiedDate]
	FROM [dbo].[Themes]
	WHERE [SearchIndexId] = @SearchIndexId

END
GO
PRINT N'Creating Procedure [Admin].[SupersedeLatestFeed]...';


GO
CREATE PROCEDURE [Admin].[SupersedeLatestFeed]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

	UPDATE dbo.Feeds
	SET IsLatest = 0,
		SupersededDate = GETUTCDATE()
	WHERE SearchIndexId = @SearchIndexId
	AND IsLatest = 1
	
END
GO
PRINT N'Creating Procedure [Admin].[SupersedeSynonym]...';


GO
CREATE PROCEDURE [Admin].[SupersedeSynonym]
(
	@SearchIndexId uniqueidentifier,
	@SynonymId uniqueidentifier
)
AS

BEGIN

	UPDATE [dbo].[Synonyms]
	SET IsLatest = 0,
		SupersededDate = GETUTCDATE()
	WHERE SearchIndexId = @SearchIndexId
	AND Id = @SynonymId
	AND IsLatest = 1
	
END
GO
PRINT N'Creating Procedure [Admin].[UpdateSynonym]...';


GO
CREATE PROCEDURE [Admin].[UpdateSynonym]
(
	@SearchIndexId uniqueidentifier,
	@SynonymId uniqueidentifier,
	@KeyWord varchar(50),
	@SolrFormat varchar(max)
)
AS

BEGIN

	UPDATE [dbo].[Synonyms]
	SET	[KeyWord] = @KeyWord,
	[SolrFormat] = @SolrFormat
	WHERE SearchIndexId = @SearchIndexId
	AND Id = @SynonymId
	AND IsLatest = 1
	
END
GO
PRINT N'Creating Procedure [Admin].[UpdateTheme]...';


GO

CREATE PROCEDURE [Admin].[UpdateTheme]
	@ThemeId uniqueidentifier,
	@PrimaryHexColour nvarchar(10),
	@SecondaryHexColour nvarchar(10),
	@NavBarHexColour nvarchar(10),
	@LogoURL nvarchar(1000),
	@MissingImageURL nvarchar(1000)
AS
BEGIN

	UPDATE [dbo].[Themes]
	SET PrimaryHexColour = @PrimaryHexColour,
		SecondaryHexColour = @SecondaryHexColour,
		NavBarHexColour = @NavBarHexColour,
		LogoURL = @LogoURL,
		MissingImageURL = @MissingImageURL,
		ModifiedDate = GETUTCDATE()
	WHERE [Id] = @ThemeId

END
GO
PRINT N'Creating Procedure [Configuration].[GetSearchIndexQueryCredentialsByCustomerEndpoint]...';


GO
CREATE PROCEDURE [Configuration].[GetSearchIndexQueryCredentialsByCustomerEndpoint]
(
	@CustomerEndpoint varchar(250)
)
AS

BEGIN

SELECT
si.Id,
LOWER(si.IndexName) as [SearchIndexName],
i.ServiceName as [SearchInstanceName],
i.[RootEndpoint] as [SearchInstanceEndpoint],
ik.ApiKey as [QueryApiKey]
FROM dbo.SearchIndex si
INNER JOIN dbo.Customers c on c.Id = si.CustomerId
INNER JOIN dbo.SearchInstances i on i.Id = si.Id
INNER JOIN dbo.SearchInstanceKeys ik on ik.Id = i.Id 
									AND ik.KeyType = 'Query' 
									AND ik.Name = 'Query key' 
									AND ik.IsLatest = 1
WHERE c.CustomerEndpoint = @CustomerEndpoint

END
GO
PRINT N'Creating Procedure [Configuration].[GetThemeByCustomerEndpoint]...';


GO
CREATE PROCEDURE [Configuration].[GetThemeByCustomerEndpoint]
(
	@CustomerEndpoint varchar(250)
)
AS

BEGIN

SELECT
t.Id,
t.PrimaryHexColour,
t.SecondaryHexColour,
t.NavBarHexColour,
t.LogoURL,
t.MissingImageURL
FROM dbo.Themes t
INNER JOIN dbo.Customers c on t.CustomerId = c.Id
WHERE c.CustomerEndpoint = @CustomerEndpoint

END
GO
PRINT N'Creating Procedure [FeedServicesFunc].[GetCurrentFeedDocuments]...';


GO
CREATE PROCEDURE [FeedServicesFunc].[GetCurrentFeedDocuments]
(
	@SearchIndexId uniqueidentifier,
	@PageNumber int,
	@PageSize int
)
AS

BEGIN

	SELECT
	[Id]
	FROM [dbo].[FeedCurrentDocuments]
	WHERE SearchIndexId = @SearchIndexId
	ORDER BY CreatedDate ASC
	OFFSET (@PageNumber - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY;

END
GO
PRINT N'Creating Procedure [FeedServicesFunc].[GetCurrentFeedDocumentsTotal]...';


GO
CREATE PROCEDURE [FeedServicesFunc].[GetCurrentFeedDocumentsTotal]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

	SELECT 
	COUNT(1) AS TotalDocuments
	FROM [dbo].[FeedCurrentDocuments] 
	WHERE SearchIndexId = @SearchIndexId

END
GO
PRINT N'Creating Procedure [FeedServicesFunc].[GetFeedCredentialsUsername]...';


GO
CREATE PROCEDURE [FeedServicesFunc].[GetFeedCredentialsUsername]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

SELECT TOP 1
fc.SearchIndexId,
fc.Username,
fc.CreatedDate,
fc.ModifiedDate
FROM dbo.FeedCredentials fc
WHERE fc.SearchIndexId = @SearchIndexId

END
GO
PRINT N'Creating Procedure [FeedServicesFunc].[GetFeedDataFormat]...';


GO
CREATE PROCEDURE [FeedServicesFunc].[GetFeedDataFormat]
(
	@CustomerId uniqueidentifier,
	@SearchIndexName varchar(60)
)
AS

BEGIN

SELECT TOP 1
f.DataFormat
FROM dbo.SearchIndex si
INNER JOIN dbo.Feeds f on f.SearchIndexId = si.Id AND f.IsLatest = 1
WHERE si.CustomerId = @CustomerId
AND si.IndexName = @SearchIndexName

END
GO
PRINT N'Creating Procedure [FeedServicesFunc].[GetLatestGenericSynonymsByCategory]...';


GO
CREATE PROCEDURE [FeedServicesFunc].[GetLatestGenericSynonymsByCategory]
(
	@Category varchar(50)
)
AS

BEGIN

SELECT
Id,
Category,
SolrFormat,
CreatedDate
FROM [dbo].[Synonyms]
WHERE Category = @Category
AND IsLatest = 1

END
GO
PRINT N'Creating Procedure [FeedServicesFunc].[GetSearchIndexCredentials]...';


GO
CREATE PROCEDURE [FeedServicesFunc].[GetSearchIndexCredentials]
(
	@CustomerId uniqueidentifier,
	@SearchIndexName varchar(60)
)
AS

BEGIN

SELECT
si.Id,
LOWER(si.IndexName) as [SearchIndexName],
i.Id,
i.ServiceName as [SearchInstanceName],
i.RootEndpoint,
ik.ApiKey
FROM dbo.SearchIndex si
INNER JOIN dbo.SearchInstances i on i.Id = si.Id
INNER JOIN dbo.SearchInstanceKeys ik on ik.Id = i.Id 
									AND ik.KeyType = 'Admin' 
									AND ik.Name = 'Primary Admin key' 
									AND ik.IsLatest = 1
WHERE si.CustomerId = @CustomerId
AND si.IndexName = @SearchIndexName

END
GO
PRINT N'Creating Procedure [FeedServicesFunc].[GetSearchIndexFeedProcessingData]...';


GO
CREATE PROCEDURE [FeedServicesFunc].[GetSearchIndexFeedProcessingData]
(
	@CustomerId uniqueidentifier,
	@SearchIndexName varchar(60)
)
AS

BEGIN

SELECT
si.Id,
LOWER(si.IndexName) as [SearchIndexName],
i.Id,
i.ServiceName as [SearchInstanceName],
i.RootEndpoint,
ik.ApiKey
FROM dbo.SearchIndex si
INNER JOIN dbo.SearchInstances i on i.Id = si.Id
INNER JOIN dbo.SearchInstanceKeys ik on ik.Id = i.Id 
									AND ik.KeyType = 'Admin' 
									AND ik.Name = 'Primary Admin key' 
									AND ik.IsLatest = 1
WHERE si.CustomerId = @CustomerId
AND si.IndexName = @SearchIndexName

SELECT TOP 1
f.DataFormat as FeedDataFormat,
c.CustomerEndpoint
FROM dbo.SearchIndex si
INNER JOIN dbo.Feeds f on f.SearchIndexId = si.Id
INNER JOIN dbo.Customers c on si.CustomerId = c.id
WHERE si.CustomerId = @CustomerId
AND si.IndexName = @SearchIndexName

END
GO
PRINT N'Creating Procedure [FeedServicesFunc].[MergeFeedDocuments]...';


GO
CREATE PROCEDURE [FeedServicesFunc].[MergeFeedDocuments]
(
    @SearchIndexId uniqueidentifier,
    @NewFeedDocuments [NewFeedDocuments] READONLY
)
AS
BEGIN
    DECLARE @UtcNow datetime = GETUTCDATE();

    MERGE [dbo].[FeedCurrentDocuments] WITH (SERIALIZABLE) AS target
    USING @NewFeedDocuments AS source
    ON @SearchIndexId = target.SearchIndexId
        AND source.DocumentId = target.Id
    WHEN MATCHED THEN
        UPDATE SET target.[CreatedDate] = @UtcNow
    WHEN NOT MATCHED BY target THEN
        INSERT ([Id], [SearchIndexId])
        VALUES (source.[DocumentId], @SearchIndexId)
    WHEN NOT MATCHED BY source AND target.SearchIndexId = @SearchIndexId THEN
        DELETE;
END
GO
PRINT N'Creating Procedure [SearchInsightsFunc].[AddDataPoints]...';


GO
CREATE PROCEDURE [SearchInsightsFunc].[AddDataPoints]
(
    @SearchIndexId uniqueidentifier,
    @SearchInsightsData [SearchInsightsData] READONLY
)
AS
BEGIN
    DECLARE @UtcNow datetime = GETUTCDATE();

    MERGE [SearchInsightsData] WITH (SERIALIZABLE) AS target
    USING @SearchInsightsData AS source
    ON @SearchIndexId = target.SearchIndexId
        AND source.[DataCategory] = target.[DataCategory]
        AND source.[DataPoint] = target.[DataPoint]
        AND source.[Date] = target.[Date]
    WHEN MATCHED THEN
        UPDATE SET 
            target.[Count] = target.[Count] + 1,
            target.[ModifiedDate] = @UtcNow
    WHEN NOT MATCHED THEN
        INSERT ([SearchIndexId], [DataCategory], [DataPoint], [Count], [Date], [ModifiedDate])
        VALUES (@SearchIndexId, source.[DataCategory], source.[DataPoint], 1, source.[Date], @UtcNow);
END
GO
PRINT N'Creating Procedure [SearchInsightsFunc].[AddSearchRequest]...';


GO
CREATE PROCEDURE [SearchInsightsFunc].[AddSearchRequest]
(
	@SearchIndexId uniqueidentifier,
	@Date date
)
AS

BEGIN
	DECLARE @UtcNow datetime = GETUTCDATE();

	MERGE [SearchIndexRequestLog] WITH (SERIALIZABLE) as target
	USING (	SELECT @SearchIndexId as [SearchIndexId], @Date as [Date]) as source
	ON source.[SearchIndexId] = target.[SearchIndexId]
	AND source.[Date] = target.[Date]
	WHEN MATCHED THEN
	UPDATE SET	target.[Count] = target.[Count] + 1, 
				target.[ModifiedDate] = @UtcNow
	WHEN NOT MATCHED THEN
	INSERT ([SearchIndexId], [Count], [Date], [ModifiedDate])
	VALUES (@SearchIndexId, 1, @Date, @UtcNow);

END
GO
PRINT N'Creating Procedure [SFTPGoServicesFunc].[AddFeedCredentials]...';


GO

CREATE PROCEDURE [SFTPGoServicesFunc].[AddFeedCredentials]
(
	@SearchIndexId uniqueidentifier,
	@Username varchar(50),
	@PasswordHash varchar(250)
)
AS

BEGIN
	
	INSERT INTO dbo.FeedCredentials
	(
		Id,
		SearchIndexId,
		Username,
		PasswordHash,
		CreatedDate
	)
	VALUES
	(
		NEWID(),
		@SearchIndexId,
		@Username,
		@PasswordHash,
		GETUTCDATE()
	)

END
GO
PRINT N'Creating Procedure [SFTPGoServicesFunc].[DeleteFeedCredentials]...';


GO

CREATE PROCEDURE [SFTPGoServicesFunc].[DeleteFeedCredentials]
(
	@SearchIndexId uniqueidentifier,
	@Username varchar(50)
)
AS

BEGIN
	
	DELETE FROM dbo.FeedCredentials
	WHERE SearchIndexId = @SearchIndexId
	AND Username = @Username

END
GO
PRINT N'Creating Procedure [SFTPGoServicesFunc].[GetFeedCredentials]...';


GO
CREATE PROCEDURE [SFTPGoServicesFunc].[GetFeedCredentials]
(
	@SearchIndexId uniqueidentifier,
	@Username varchar(50)
)
AS

BEGIN
	
	SELECT
	SearchIndexId,
	Username,
	CreatedDate
	FROM [dbo].[FeedCredentials]
	WHERE SearchIndexId = @SearchIndexId
	AND Username = @Username

END
GO
PRINT N'Creating Procedure [SFTPGoServicesFunc].[UpdateFeedCredentials]...';


GO
CREATE PROCEDURE [SFTPGoServicesFunc].[UpdateFeedCredentials]
(
	@SearchIndexId uniqueidentifier,
	@Username varchar(50),
	@PasswordHash varchar(250)
)
AS

BEGIN
	
	UPDATE dbo.FeedCredentials
	SET	PasswordHash = @PasswordHash,
		ModifiedDate = GETUTCDATE()
	WHERE SearchIndexId = @SearchIndexId
	AND Username = @Username

END
GO
PRINT N'Creating Procedure [Admin].[AddFeed]...';


GO
CREATE PROCEDURE [Admin].[AddFeed]
(
	@SearchIndexId uniqueidentifier,
	@FeedType varchar(20),
	@FeedCron varchar(255)
)
AS

BEGIN

	EXEC [Admin].[SupersedeLatestFeed] @SearchIndexId = @SearchIndexId
	
	INSERT INTO dbo.Feeds
	(
		SearchIndexId,
		FeedType,
		FeedScheduleCron
	)
	VALUES
	(
		@SearchIndexId,
		@FeedType,
		@FeedCron
	)

END
GO
PRINT N'Update complete.';


GO