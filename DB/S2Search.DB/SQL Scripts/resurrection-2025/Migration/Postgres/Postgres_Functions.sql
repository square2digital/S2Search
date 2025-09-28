-- =============================
-- Drop Function Definitions
-- =============================

DROP FUNCTION IF EXISTS AddSearchIndex(uuid, uuid, uuid, TEXT, TEXT);
DROP FUNCTION IF EXISTS AddSynonym(uuid, uuid, TEXT, TEXT);
DROP FUNCTION IF EXISTS GetCustomerByID(uuid);
DROP FUNCTION IF EXISTS GetCustomerFull(uuid);
DROP FUNCTION IF EXISTS GetLatestFeed(uuid);
DROP FUNCTION IF EXISTS GetSearchIndex(uuid, uuid);
DROP FUNCTION IF EXISTS GetSearchIndexByFriendlyName(uuid, TEXT);
DROP FUNCTION IF EXISTS GetSearchIndexFull(uuid, uuid);
DROP FUNCTION IF EXISTS GetSearchIndexQueryCredentialsByCustomerEndpoint(TEXT);
DROP FUNCTION IF EXISTS GetSearchInsightsByDataCateries(uuid, TIMESTAMP, TIMESTAMP, TEXT);
DROP FUNCTION IF EXISTS GetSearchInsightsSearchCountByDateRange(uuid, TIMESTAMP, TIMESTAMP);
DROP FUNCTION IF EXISTS GetSynonymById(uuid, uuid);
DROP FUNCTION IF EXISTS GetSynonymByKeyWord(uuid, TEXT);
DROP FUNCTION IF EXISTS GetSynonyms(uuid);
DROP FUNCTION IF EXISTS GetThemeByCustomerId(uuid);
DROP FUNCTION IF EXISTS GetThemeById(uuid);
DROP FUNCTION IF EXISTS GetThemeBySearchIndexId(uuid);
DROP FUNCTION IF EXISTS SupersedeLatestFeed(uuid);
DROP FUNCTION IF EXISTS SupersedeSynonym(uuid, uuid);
DROP FUNCTION IF EXISTS UpdateSynonym(uuid, uuid, TEXT, TEXT);
DROP FUNCTION IF EXISTS UpdateTheme(uuid, TEXT, TEXT, TEXT, TEXT, TEXT);
DROP FUNCTION IF EXISTS GetSearchIndexQueryCredentialsByCustomerEndpoint(TEXT);
DROP FUNCTION IF EXISTS GetCurrentFeedDocuments(uuid, INT, INT);
DROP FUNCTION IF EXISTS GetCurrentFeedDocumentsTotal(uuid);
DROP FUNCTION IF EXISTS GetFeedCredentialsUsername(uuid);
DROP FUNCTION IF EXISTS GetFeedDataFormat(uuid, TEXT);
DROP FUNCTION IF EXISTS GetLatestGenericSynonymsByCatery(TEXT);
DROP FUNCTION IF EXISTS GetSearchIndexCredentials(uuid, TEXT);
DROP FUNCTION IF EXISTS GetSearchIndexFeedProcessingData(uuid, TEXT);
DROP FUNCTION IF EXISTS MergeFeedDocuments(uuid, NewFeedDocuments);
DROP FUNCTION IF EXISTS AddDataPoints(uuid, SearchInsightsData);
DROP FUNCTION IF EXISTS AddSearchRequest(uuid, DATE);
DROP FUNCTION IF EXISTS AddFeedCredentials(uuid, TEXT, TEXT);
DROP FUNCTION IF EXISTS DeleteFeedCredentials(uuid, TEXT);
DROP FUNCTION IF EXISTS GetFeedCredentials(uuid, TEXT);
DROP FUNCTION IF EXISTS UpdateFeedCredentials(uuid, TEXT, TEXT);
DROP FUNCTION IF EXISTS AddFeed(uuid, TEXT, TEXT);

-- =============================
-- Function Definitions
-- =============================

CREATE OR REPLACE FUNCTION GetCustomerByID(CustomerId uuid)
RETURNS TABLE (Id uuid, BusinessName text) AS $$
BEGIN
    RETURN QUERY
    SELECT c.Id, c.BusinessName
    FROM Customers c
    WHERE c.Id = CustomerId;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetCustomerFull(CustomerId uuid)
RETURNS TABLE (
    Id uuid,
    BusinessName text
) AS $$
BEGIN
    RETURN QUERY
    SELECT c.Id, c.BusinessName
    FROM Customers c
    WHERE c.Id = CustomerId;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetLatestFeed(SearchIndexId uuid)
RETURNS TABLE (
    Id uuid,
    SearchIndexId uuid,
    FeedType text,
    FeedScheduleCron text,
    CreatedDate timestamp,
    SupersededDate timestamp,
    IsLatest boolean
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        f.Id,
        f.SearchIndexId,
        f.FeedType,
        f.FeedScheduleCron,
        f.CreatedDate,
        f.SupersededDate,
        f.IsLatest
    FROM dbo.Feeds f
    WHERE f.SearchIndexId = SearchIndexId
      AND f.IsLatest = 1
    LIMIT 1;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndex(
    SearchIndexId uuid,
    CustomerId uuid
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT
search.Id,
search.CustomerId,
search.IndexName,
search.FriendlyName,
service.RootEndpoint,
service.PricingTier,
search.CreatedDate,
service.Id,
service.ServiceName,
service.Location,
service.PricingTier,
service.Replicas,
service.Partitions,
service.IsShared
FROM dbo.SearchIndex search
LEFT OUTER JOIN dbo.SearchInstances service on service.Id = search.SearchInstanceId
WHERE search.Id = SearchIndexId
AND search.CustomerId = CustomerId

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndexByFriendlyName(
    CustomerId uuid,
    FriendlyName TEXT(100)
) RETURNS TABLE (...) AS $$
BEGIN
(
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
WHERE si.CustomerId = CustomerId
AND si.FriendlyName = FriendlyName

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndexFull(
    SearchIndexId uuid,
    CustomerId uuid
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT
search.Id,
search.CustomerId,
search.IndexName,
search.FriendlyName,
service.RootEndpoint,
service.PricingTier,
search.CreatedDate,
service.Id,
service.ServiceName,
service.Location,
service.PricingTier,
service.Replicas,
service.Partitions,
service.IsShared
FROM dbo.SearchIndex search
LEFT OUTER JOIN dbo.SearchInstances service on service.Id = search.Id
WHERE search.Id = SearchIndexId
AND search.CustomerId = CustomerId

--If no results it didnt match on SearchIndexId and CustomerId so override the SearchIndexId so that the other selects do not return a result
IF ROWCOUNT = 0
BEGIN
	SET SearchIndexId = NULL
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndexQueryCredentialsByCustomerEndpoint(
    CustomerEndpoint TEXT(250)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT
si.Id,
LOWER(si.IndexName) as SearchIndexName,
i.ServiceName as SearchInstanceName,
i.RootEndpoint as SearchInstanceEndpoint,
ik.ApiKey
FROM dbo.SearchIndex si
INNER JOIN dbo.SearchInstances i on i.Id = si.SearchInstanceId
INNER JOIN dbo.Customers c on si.CustomerId = c.Id
INNER JOIN dbo.SearchInstanceKeys ik on ik.SearchInstanceId = i.Id
									AND ik.KeyType = 'Query'
									AND ik.Name = 'Query key'
									AND ik.IsLatest = 1
WHERE c.CustomerEndpoint = CustomerEndpoint

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchInsightsByDataCategories(
    SearchIndexId uuid,
    DateFrom TIMESTAMP,
    DateTo TIMESTAMP,
    DataCategories TEXT(1000)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT
d.DataCategory,
d.DataPoint,
d.Date,
d.Count
FROM dbo.SearchInsightsData d
CROSS APPLY string_split(DataCategories, ',') categories
WHERE d.SearchIndexId = SearchIndexId
AND d.Date >= DateFrom
AND d.Date <= DateTo
AND categories.value = d.DataCategory

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchInsightsSearchCountByDateRange(
    SearchIndexId uuid,
    DateFrom TIMESTAMP,
    DateTo TIMESTAMP
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT
d.Date,
d.Count
FROM dbo.SearchIndexRequestLog d
WHERE d.SearchIndexId = SearchIndexId
AND d.Date BETWEEN DateFrom AND DateTo

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSynonymById(
    SearchIndexId uuid,
    SynonymId uuid
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT
Id,
SearchIndexId,
KeyWord as Key,
SolrFormat,
CreatedDate
FROM dbo.Synonyms
WHERE SearchIndexId = SearchIndexId
AND Id = SynonymId
AND IsLatest = 1

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSynonymByKeyWord(
    SearchIndexId uuid,
    KeyWord TEXT(30)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT
Id,
SearchIndexId,
KeyWord as Key,
SolrFormat,
CreatedDate
FROM dbo.Synonyms
WHERE SearchIndexId = SearchIndexId
AND KeyWord = KeyWord
AND IsLatest = 1

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSynonyms(
    SearchIndexId uuid
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT
Id,
SearchIndexId,
KeyWord as Key,
SolrFormat,
CreatedDate
FROM dbo.Synonyms
WHERE SearchIndexId = SearchIndexId
AND IsLatest = 1

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetThemeByCustomerId(
    CustomerId uuid
) RETURNS TABLE (...) AS $$
BEGIN
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id
      ,PrimaryHexColour
      ,SecondaryHexColour
      ,NavBarHexColour
      ,LogoURL
	  ,MissingImageURL
      ,CustomerId
      ,SearchIndexId
      ,CreatedDate
      ,ModifiedDate
	FROM dbo.Themes
	WHERE CustomerId = CustomerId

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetThemeById(
    ThemeId uuid
) RETURNS TABLE (...) AS $$
BEGIN
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id
      ,PrimaryHexColour
      ,SecondaryHexColour
      ,NavBarHexColour
      ,LogoURL
	  ,MissingImageURL
      ,CustomerId
      ,SearchIndexId
      ,CreatedDate
      ,ModifiedDate
	FROM dbo.Themes
	WHERE Id = ThemeId

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetThemeBySearchIndexId(
    SearchIndexId uuid
) RETURNS TABLE (...) AS $$
BEGIN
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id
      ,PrimaryHexColour
      ,SecondaryHexColour
      ,NavBarHexColour
      ,LogoURL
	  ,MissingImageURL
      ,CustomerId
      ,SearchIndexId
      ,CreatedDate
      ,ModifiedDate
	FROM dbo.Themes
	WHERE SearchIndexId = SearchIndexId

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION SupersedeLatestFeed(
    SearchIndexId uuid
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

	UPDATE dbo.Feeds
	SET IsLatest = 0,
		SupersededDate = CURRENT_TIMESTAMP
	WHERE SearchIndexId = SearchIndexId
	AND IsLatest = 1

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION SupersedeSynonym(
    SearchIndexId uuid,
    SynonymId uuid
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

	UPDATE dbo.Synonyms
	SET IsLatest = 0,
		SupersededDate = CURRENT_TIMESTAMP
	WHERE SearchIndexId = SearchIndexId
	AND Id = SynonymId
	AND IsLatest = 1

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION UpdateSynonym(
    SearchIndexId uuid,
    SynonymId uuid,
    KeyWord TEXT(50),
    SolrFormat TEXT(max)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

	UPDATE dbo.Synonyms
	SET	KeyWord = KeyWord,
	SolrFormat = SolrFormat
	WHERE SearchIndexId = SearchIndexId
	AND Id = SynonymId
	AND IsLatest = 1

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION UpdateTheme(
    ThemeId uuid,
    PrimaryHexColour nTEXT(10),
    SecondaryHexColour nTEXT(10),
    NavBarHexColour nTEXT(10),
    LogoURL nTEXT(1000),
    MissingImageURL nTEXT(1000)
) RETURNS TABLE (...) AS $$
BEGIN
AS
BEGIN

	UPDATE dbo.Themes
	SET PrimaryHexColour = PrimaryHexColour,
		SecondaryHexColour = SecondaryHexColour,
		NavBarHexColour = NavBarHexColour,
		LogoURL = LogoURL,
		MissingImageURL = MissingImageURL,
		ModifiedDate = CURRENT_TIMESTAMP
	WHERE Id = ThemeId

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndexQueryCredentialsByCustomerEndpoint(
    CustomerEndpoint TEXT(250)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT
si.Id,
LOWER(si.IndexName) as SearchIndexName,
i.ServiceName as SearchInstanceName,
i.RootEndpoint as SearchInstanceEndpoint,
ik.ApiKey as QueryApiKey
FROM dbo.SearchIndex si
INNER JOIN dbo.Customers c on c.Id = si.CustomerId
INNER JOIN dbo.SearchInstances i on i.Id = si.Id
INNER JOIN dbo.SearchInstanceKeys ik on ik.Id = i.Id
									AND ik.KeyType = 'Query'
									AND ik.Name = 'Query key'
									AND ik.IsLatest = 1
WHERE c.CustomerEndpoint = CustomerEndpoint

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetThemeByCustomerEndpoint(
    CustomerEndpoint TEXT(250)
) RETURNS TABLE (...) AS $$
BEGIN
(
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
WHERE c.CustomerEndpoint = CustomerEndpoint

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetCurrentFeedDocuments(
    SearchIndexId uuid,
    PageNumber int,
    PageSize int
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

	SELECT
	Id
	FROM dbo.FeedCurrentDocuments
	WHERE SearchIndexId = SearchIndexId
	ORDER BY CreatedDate ASC
	OFFSET (PageNumber - 1) * PageSize ROWS
	FETCH NEXT PageSize ROWS ONLY;

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetCurrentFeedDocumentsTotal(
    SearchIndexId uuid
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

	SELECT
	COUNT(1) AS TotalDocuments
	FROM dbo.FeedCurrentDocuments
	WHERE SearchIndexId = SearchIndexId

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetFeedCredentialsUsername(
    SearchIndexId uuid
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT LIMIT 1
fc.SearchIndexId,
fc.Username,
fc.CreatedDate,
fc.ModifiedDate
FROM dbo.FeedCredentials fc
WHERE fc.SearchIndexId = SearchIndexId

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetFeedDataFormat(
    CustomerId uuid,
    SearchIndexName TEXT(60)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT LIMIT 1
f.DataFormat
FROM dbo.SearchIndex si
INNER JOIN dbo.Feeds f on f.SearchIndexId = si.Id AND f.IsLatest = 1
WHERE si.CustomerId = CustomerId
AND si.IndexName = SearchIndexName

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetLatestGenericSynonymsByCategory(
    Category TEXT(50)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT
Id,
Category,
SolrFormat,
CreatedDate
FROM dbo.Synonyms
WHERE Category = Category
AND IsLatest = 1

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndexCredentials(
    CustomerId uuid,
    SearchIndexName TEXT(60)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT
si.Id,
LOWER(si.IndexName) as SearchIndexName,
i.Id,
i.ServiceName as SearchInstanceName,
i.RootEndpoint,
ik.ApiKey
FROM dbo.SearchIndex si
INNER JOIN dbo.SearchInstances i on i.Id = si.Id
INNER JOIN dbo.SearchInstanceKeys ik on ik.Id = i.Id
									AND ik.KeyType = 'Admin'
									AND ik.Name = 'Primary Admin key'
									AND ik.IsLatest = 1
WHERE si.CustomerId = CustomerId
AND si.IndexName = SearchIndexName

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndexFeedProcessingData(
    CustomerId uuid,
    SearchIndexName TEXT(60)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

SELECT
si.Id,
LOWER(si.IndexName) as SearchIndexName,
i.Id,
i.ServiceName as SearchInstanceName,
i.RootEndpoint,
ik.ApiKey
FROM dbo.SearchIndex si
INNER JOIN dbo.SearchInstances i on i.Id = si.Id
INNER JOIN dbo.SearchInstanceKeys ik on ik.Id = i.Id
									AND ik.KeyType = 'Admin'
									AND ik.Name = 'Primary Admin key'
									AND ik.IsLatest = 1
WHERE si.CustomerId = CustomerId
AND si.IndexName = SearchIndexName

SELECT LIMIT 1
f.DataFormat as FeedDataFormat,
c.CustomerEndpoint
FROM dbo.SearchIndex si
INNER JOIN dbo.Feeds f on f.SearchIndexId = si.Id
INNER JOIN dbo.Customers c on si.CustomerId = c.id
WHERE si.CustomerId = CustomerId
AND si.IndexName = SearchIndexName

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION MergeFeedDocuments(
    SearchIndexId uuid,
    NewFeedDocuments [newfeeddocuments]
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS
BEGIN
    DECLARE UtcNow datetime = CURRENT_TIMESTAMP;

    MERGE dbo.FeedCurrentDocuments WITH (SERIALIZABLE) AS target
    USING NewFeedDocuments AS source
    ON SearchIndexId = target.SearchIndexId
        AND source.DocumentId = target.Id
    WHEN MATCHED THEN
        UPDATE SET target.CreatedDate = UtcNow
    WHEN NOT MATCHED BY target THEN
        INSERT (Id, SearchIndexId)
        VALUES (source.DocumentId, SearchIndexId)
    WHEN NOT MATCHED BY source AND target.SearchIndexId = SearchIndexId THEN
        DELETE;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION AddDataPoints(
    SearchIndexId uuid,
    SearchInsightsData [searchinsightsdata]
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS
BEGIN
    DECLARE UtcNow datetime = CURRENT_TIMESTAMP;

    MERGE SearchInsightsData WITH (SERIALIZABLE) AS target
    USING SearchInsightsData AS source
    ON SearchIndexId = target.SearchIndexId
        AND source.DataCategory = target.DataCategory
        AND source.DataPoint = target.DataPoint
        AND source.Date = target.Date
    WHEN MATCHED THEN
        UPDATE SET
            target.Count = target.Count + 1,
            target.ModifiedDate = UtcNow
    WHEN NOT MATCHED THEN
        INSERT (SearchIndexId, DataCategory, DataPoint, Count, Date, ModifiedDate)
        VALUES (SearchIndexId, source.DataCategory, source.DataPoint, 1, source.Date, UtcNow);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION AddSearchRequest(
    SearchIndexId uuid,
    Date date
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN
	DECLARE UtcNow datetime = CURRENT_TIMESTAMP;

	MERGE SearchIndexRequestLog WITH (SERIALIZABLE) as target
	USING (	SELECT SearchIndexId as SearchIndexId, Date as Date) as source
	ON source.SearchIndexId = target.SearchIndexId
	AND source.Date = target.Date
	WHEN MATCHED THEN
	UPDATE SET	target.Count = target.Count + 1,
				target.ModifiedDate = UtcNow
	WHEN NOT MATCHED THEN
	INSERT (SearchIndexId, Count, Date, ModifiedDate)
	VALUES (SearchIndexId, 1, Date, UtcNow);

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION AddFeedCredentials(
    SearchIndexId uuid,
    Username TEXT(50),
    PasswordHash TEXT(250)
) RETURNS TABLE (...) AS $$
BEGIN
(
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
		gen_random_uuid(),
		CURRENT_TIMESTAMP
	)

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION DeleteFeedCredentials(
    SearchIndexId uuid,
    Username TEXT(50)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

	DELETE FROM dbo.FeedCredentials
	WHERE SearchIndexId = SearchIndexId
	AND Username = Username

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetFeedCredentials(
    SearchIndexId uuid,
    Username TEXT(50)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

	SELECT
	SearchIndexId,
	Username,
	CreatedDate
	FROM dbo.FeedCredentials
	WHERE SearchIndexId = SearchIndexId
	AND Username = Username

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION UpdateFeedCredentials(
    SearchIndexId uuid,
    Username TEXT(50),
    PasswordHash TEXT(250)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

	UPDATE dbo.FeedCredentials
	SET	PasswordHash = PasswordHash,
		ModifiedDate = CURRENT_TIMESTAMP
	WHERE SearchIndexId = SearchIndexId
	AND Username = Username

END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION AddFeed(
    SearchIndexId uuid,
    FeedType TEXT(20),
    FeedCron TEXT(255)
) RETURNS TABLE (...) AS $$
BEGIN
(
)
AS

BEGIN

	EXEC Admin.SupersedeLatestFeed SearchIndexId = SearchIndexId

	INSERT INTO dbo.Feeds
	(
		SearchIndexId,
		FeedType,
		FeedScheduleCron
	)
	VALUES
	(
	)

END;
$$ LANGUAGE plpgsql;
