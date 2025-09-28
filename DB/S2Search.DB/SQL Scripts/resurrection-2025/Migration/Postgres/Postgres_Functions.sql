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
) RETURNS TABLE (
    Id uuid,
    CustomerId uuid,
    IndexName text,
    FriendlyName text,
    RootEndpoint text,
    PricingTier text,
    CreatedDate timestamp,
    InstanceId uuid,
    ServiceName text,
    Location text,
    InstancePricingTier text,
    Replicas int,
    Partitions int,
    IsShared boolean
) AS $$
BEGIN
    RETURN QUERY
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
    LEFT OUTER JOIN dbo.SearchInstances service ON service.Id = search.SearchInstanceId
    WHERE search.Id = SearchIndexId
      AND search.CustomerId = CustomerId;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndexByFriendlyName(
    CustomerId uuid,
    FriendlyName TEXT
) RETURNS TABLE (
    Id uuid,
    SearchInstanceId uuid,
    CustomerId uuid,
    FriendlyName text,
    IndexName text
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        si.Id,
        si.SearchInstanceId,
        si.CustomerId,
        si.FriendlyName,
        si.IndexName
    FROM dbo.SearchIndex si
    WHERE si.CustomerId = CustomerId
      AND si.FriendlyName = FriendlyName;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndexFull(
    SearchIndexId uuid,
    CustomerId uuid
) RETURNS TABLE (
    Id uuid,
    CustomerId uuid,
    IndexName text,
    FriendlyName text,
    RootEndpoint text,
    PricingTier text,
    CreatedDate timestamp,
    InstanceId uuid,
    ServiceName text,
    Location text,
    InstancePricingTier text,
    Replicas int,
    Partitions int,
    IsShared boolean
) AS $$
BEGIN
    RETURN QUERY
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
    LEFT OUTER JOIN dbo.SearchInstances service ON service.Id = search.Id
    WHERE search.Id = SearchIndexId
      AND search.CustomerId = CustomerId;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndexQueryCredentialsByCustomerEndpoint(
    CustomerEndpoint TEXT
) RETURNS TABLE (
    Id uuid,
    SearchIndexName text,
    SearchInstanceName text,
    SearchInstanceEndpoint text,
    ApiKey text
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        si.Id,
        LOWER(si.IndexName) as SearchIndexName,
        i.ServiceName as SearchInstanceName,
        i.RootEndpoint as SearchInstanceEndpoint,
        ik.ApiKey
    FROM dbo.SearchIndex si
    INNER JOIN dbo.SearchInstances i ON i.Id = si.SearchInstanceId
    INNER JOIN dbo.Customers c ON si.CustomerId = c.Id
    INNER JOIN dbo.SearchInstanceKeys ik ON ik.SearchInstanceId = i.Id
        AND ik.KeyType = 'Query'
        AND ik.Name = 'Query key'
        AND ik.IsLatest = 1
    WHERE c.CustomerEndpoint = CustomerEndpoint;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchInsightsByDataCategories(
    SearchIndexId uuid,
    DateFrom TIMESTAMP,
    DateTo TIMESTAMP,
    DataCategories TEXT
) RETURNS TABLE (
    DataCategory text,
    DataPoint text,
    Date timestamp,
    Count int
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        d.DataCategory,
        d.DataPoint,
        d.Date,
        d.Count
    FROM dbo.SearchInsightsData d
    JOIN unnest(string_to_array(DataCategories, ',')) AS categories(category)
        ON d.DataCategory = categories.category
    WHERE d.SearchIndexId = SearchIndexId
      AND d.Date >= DateFrom
      AND d.Date <= DateTo;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchInsightsSearchCountByDateRange(
    SearchIndexId uuid,
    DateFrom TIMESTAMP,
    DateTo TIMESTAMP
) RETURNS TABLE (
    Date timestamp,
    Count int
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        d.Date,
        d.Count
    FROM dbo.SearchIndexRequestLog d
    WHERE d.SearchIndexId = SearchIndexId
      AND d.Date BETWEEN DateFrom AND DateTo;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSynonymById(
    SearchIndexId uuid,
    SynonymId uuid
) RETURNS TABLE (
    Id uuid,
    SearchIndexId uuid,
    Key text,
    SolrFormat text,
    CreatedDate timestamp
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        Id,
        SearchIndexId,
        KeyWord as Key,
        SolrFormat,
        CreatedDate
    FROM dbo.Synonyms
    WHERE SearchIndexId = SearchIndexId
      AND Id = SynonymId
      AND IsLatest = 1;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSynonymByKeyWord(
    SearchIndexId uuid,
    KeyWord TEXT
) RETURNS TABLE (
    Id uuid,
    SearchIndexId uuid,
    Key text,
    SolrFormat text,
    CreatedDate timestamp
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        Id,
        SearchIndexId,
        KeyWord as Key,
        SolrFormat,
        CreatedDate
    FROM dbo.Synonyms
    WHERE SearchIndexId = SearchIndexId
      AND KeyWord = KeyWord
      AND IsLatest = 1;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSynonyms(
    SearchIndexId uuid
) RETURNS TABLE (
    Id uuid,
    SearchIndexId uuid,
    Key text,
    SolrFormat text,
    CreatedDate timestamp
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        Id,
        SearchIndexId,
        KeyWord as Key,
        SolrFormat,
        CreatedDate
    FROM dbo.Synonyms
    WHERE SearchIndexId = SearchIndexId
      AND IsLatest = 1;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetThemeByCustomerId(
    CustomerId uuid
) RETURNS TABLE (
    Id uuid,
    PrimaryHexColour text,
    SecondaryHexColour text,
    NavBarHexColour text,
    LogoURL text,
    MissingImageURL text,
    CustomerId uuid,
    SearchIndexId uuid,
    CreatedDate timestamp,
    ModifiedDate timestamp
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        Id,
        PrimaryHexColour,
        SecondaryHexColour,
        NavBarHexColour,
        LogoURL,
        MissingImageURL,
        CustomerId,
        SearchIndexId,
        CreatedDate,
        ModifiedDate
    FROM dbo.Themes
    WHERE CustomerId = CustomerId;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetThemeById(
    ThemeId uuid
) RETURNS TABLE (
    Id uuid,
    PrimaryHexColour text,
    SecondaryHexColour text,
    NavBarHexColour text,
    LogoURL text,
    MissingImageURL text,
    CustomerId uuid,
    SearchIndexId uuid,
    CreatedDate timestamp,
    ModifiedDate timestamp
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        Id,
        PrimaryHexColour,
        SecondaryHexColour,
        NavBarHexColour,
        LogoURL,
        MissingImageURL,
        CustomerId,
        SearchIndexId,
        CreatedDate,
        ModifiedDate
    FROM dbo.Themes
    WHERE Id = ThemeId;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetThemeBySearchIndexId(
    SearchIndexId uuid
) RETURNS TABLE (
    Id uuid,
    PrimaryHexColour text,
    SecondaryHexColour text,
    NavBarHexColour text,
    LogoURL text,
    MissingImageURL text,
    CustomerId uuid,
    SearchIndexId uuid,
    CreatedDate timestamp,
    ModifiedDate timestamp
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        Id,
        PrimaryHexColour,
        SecondaryHexColour,
        NavBarHexColour,
        LogoURL,
        MissingImageURL,
        CustomerId,
        SearchIndexId,
        CreatedDate,
        ModifiedDate
    FROM dbo.Themes
    WHERE SearchIndexId = SearchIndexId;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION SupersedeLatestFeed(
    SearchIndexId uuid
) RETURNS int AS $$
DECLARE
    rows_updated int;
BEGIN
    UPDATE dbo.Feeds
    SET IsLatest = 0,
        SupersededDate = CURRENT_TIMESTAMP
    WHERE SearchIndexId = SearchIndexId
      AND IsLatest = 1;
    GET DIAGNOSTICS rows_updated = ROW_COUNT;
    RETURN rows_updated;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION SupersedeSynonym(
    SearchIndexId uuid,
    SynonymId uuid
) RETURNS int AS $$
DECLARE
    rows_updated int;
BEGIN
    UPDATE dbo.Synonyms
    SET IsLatest = 0,
        SupersededDate = CURRENT_TIMESTAMP
    WHERE SearchIndexId = SearchIndexId
      AND Id = SynonymId
      AND IsLatest = 1;
    GET DIAGNOSTICS rows_updated = ROW_COUNT;
    RETURN rows_updated;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION UpdateSynonym(
    SearchIndexId uuid,
    SynonymId uuid,
    KeyWord TEXT,
    SolrFormat TEXT
) RETURNS int AS $$
DECLARE
    rows_updated int;
BEGIN
    UPDATE dbo.Synonyms
    SET KeyWord = KeyWord,
        SolrFormat = SolrFormat
    WHERE SearchIndexId = SearchIndexId
      AND Id = SynonymId
      AND IsLatest = 1;
    GET DIAGNOSTICS rows_updated = ROW_COUNT;
    RETURN rows_updated;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION UpdateTheme(
    ThemeId uuid,
    PrimaryHexColour text,
    SecondaryHexColour text,
    NavBarHexColour text,
    LogoURL text,
    MissingImageURL text
) RETURNS int AS $$
DECLARE
    rows_updated int;
BEGIN
    UPDATE dbo.Themes
    SET PrimaryHexColour = PrimaryHexColour,
        SecondaryHexColour = SecondaryHexColour,
        NavBarHexColour = NavBarHexColour,
        LogoURL = LogoURL,
        MissingImageURL = MissingImageURL,
        ModifiedDate = CURRENT_TIMESTAMP
    WHERE Id = ThemeId;
    GET DIAGNOSTICS rows_updated = ROW_COUNT;
    RETURN rows_updated;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndexQueryCredentialsByCustomerEndpoint(
    CustomerEndpoint TEXT
) RETURNS TABLE (
    Id uuid,
    SearchIndexName text,
    SearchInstanceName text,
    SearchInstanceEndpoint text,
    QueryApiKey text
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        si.Id,
        LOWER(si.IndexName) as SearchIndexName,
        i.ServiceName as SearchInstanceName,
        i.RootEndpoint as SearchInstanceEndpoint,
        ik.ApiKey as QueryApiKey
    FROM dbo.SearchIndex si
    INNER JOIN dbo.Customers c on c.Id = si.CustomerId
    INNER JOIN dbo.SearchInstances i on i.Id = si.SearchInstanceId
    INNER JOIN dbo.SearchInstanceKeys ik on ik.SearchInstanceId = i.Id
        AND ik.KeyType = 'Query'
        AND ik.Name = 'Query key'
        AND ik.IsLatest = 1
    WHERE c.CustomerEndpoint = CustomerEndpoint;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetThemeByCustomerEndpoint(
    CustomerEndpoint TEXT
) RETURNS TABLE (
    Id uuid,
    PrimaryHexColour text,
    SecondaryHexColour text,
    NavBarHexColour text,
    LogoURL text,
    MissingImageURL text
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        t.Id,
        t.PrimaryHexColour,
        t.SecondaryHexColour,
        t.NavBarHexColour,
        t.LogoURL,
        t.MissingImageURL
    FROM dbo.Themes t
    INNER JOIN dbo.Customers c on t.CustomerId = c.Id
    WHERE c.CustomerEndpoint = CustomerEndpoint;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetCurrentFeedDocuments(
    SearchIndexId uuid,
    PageNumber int,
    PageSize int
) RETURNS TABLE (
    Id uuid
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        Id
    FROM dbo.FeedCurrentDocuments
    WHERE SearchIndexId = SearchIndexId
    ORDER BY CreatedDate ASC
    OFFSET (PageNumber - 1) * PageSize
    LIMIT PageSize;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetCurrentFeedDocumentsTotal(
    SearchIndexId uuid
) RETURNS TABLE (
    TotalDocuments bigint
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        COUNT(1) AS TotalDocuments
    FROM dbo.FeedCurrentDocuments
    WHERE SearchIndexId = SearchIndexId;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetFeedCredentialsUsername(
    SearchIndexId uuid
) RETURNS TABLE (
    SearchIndexId uuid,
    Username text,
    CreatedDate timestamp,
    ModifiedDate timestamp
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        fc.SearchIndexId,
        fc.Username,
        fc.CreatedDate,
        fc.ModifiedDate
    FROM dbo.FeedCredentials fc
    WHERE fc.SearchIndexId = SearchIndexId
    LIMIT 1;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetFeedDataFormat(
    CustomerId uuid,
    SearchIndexName TEXT
) RETURNS TABLE (
    DataFormat text
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        f.DataFormat
    FROM dbo.SearchIndex si
    INNER JOIN dbo.Feeds f ON f.SearchIndexId = si.Id AND f.IsLatest = 1
    WHERE si.CustomerId = CustomerId
      AND si.IndexName = SearchIndexName
    LIMIT 1;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetLatestGenericSynonymsByCategory(
    Category TEXT
) RETURNS TABLE (
    Id uuid,
    Category text,
    SolrFormat text,
    CreatedDate timestamp
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        Id,
        Category,
        SolrFormat,
        CreatedDate
    FROM dbo.Synonyms
    WHERE Category = Category
      AND IsLatest = 1;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndexCredentials(
    CustomerId uuid,
    SearchIndexName TEXT
) RETURNS TABLE (
    Id uuid,
    SearchIndexName text,
    InstanceId uuid,
    SearchInstanceName text,
    RootEndpoint text,
    ApiKey text
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        si.Id,
        LOWER(si.IndexName) as SearchIndexName,
        i.Id as InstanceId,
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
      AND si.IndexName = SearchIndexName;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetSearchIndexFeedProcessingData(
    CustomerId uuid,
    SearchIndexName TEXT
) RETURNS TABLE (
    Id uuid,
    SearchIndexName text,
    InstanceId uuid,
    SearchInstanceName text,
    RootEndpoint text,
    ApiKey text,
    FeedDataFormat text,
    CustomerEndpoint text
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        si.Id,
        LOWER(si.IndexName) as SearchIndexName,
        i.Id as InstanceId,
        i.ServiceName as SearchInstanceName,
        i.RootEndpoint,
        ik.ApiKey,
        f.DataFormat as FeedDataFormat,
        c.CustomerEndpoint
    FROM dbo.SearchIndex si
    INNER JOIN dbo.SearchInstances i ON i.Id = si.Id
    INNER JOIN dbo.SearchInstanceKeys ik ON ik.Id = i.Id
        AND ik.KeyType = 'Admin'
        AND ik.Name = 'Primary Admin key'
        AND ik.IsLatest = 1
    INNER JOIN dbo.Feeds f ON f.SearchIndexId = si.Id
    INNER JOIN dbo.Customers c ON si.CustomerId = c.Id
    WHERE si.CustomerId = CustomerId
      AND si.IndexName = SearchIndexName
      AND f.IsLatest = 1
    LIMIT 1;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION MergeFeedDocuments(
    SearchIndexId uuid,
    NewFeedDocuments jsonb
) RETURNS int AS $$
DECLARE
    rows_updated int := 0;
BEGIN
    -- Example: iterate over JSON array and upsert each document
    -- You must adapt this logic to your actual document structure
    -- This is a template for how to process each document
    FOR document IN SELECT * FROM jsonb_array_elements(NewFeedDocuments)
    LOOP
        -- Upsert logic here, e.g.:
        -- INSERT INTO dbo.FeedCurrentDocuments (Id, SearchIndexId, CreatedDate)
        -- VALUES (document->>'DocumentId', SearchIndexId, CURRENT_TIMESTAMP)
        -- ON CONFLICT (Id) DO UPDATE SET CreatedDate = CURRENT_TIMESTAMP;
        -- rows_updated := rows_updated + 1;
    END LOOP;
    RETURN rows_updated;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION AddDataPoints(
    SearchIndexId uuid,
    SearchInsightsData jsonb
) RETURNS int AS $$
DECLARE
    rows_inserted int := 0;
BEGIN
    -- Example: iterate over JSON array and insert each data point
    FOR data_point IN SELECT * FROM jsonb_array_elements(SearchInsightsData)
    LOOP
        -- Example insert, adapt to your table structure:
        -- INSERT INTO dbo.SearchInsightsData (SearchIndexId, DataCategory, DataPoint, Count, Date, ModifiedDate)
        -- VALUES (
        --     SearchIndexId,
        --     data_point->>'DataCategory',
        --     data_point->>'DataPoint',
        --     COALESCE((data_point->>'Count')::int, 1),
        --     (data_point->>'Date')::timestamp,
        --     CURRENT_TIMESTAMP
        -- );
        -- rows_inserted := rows_inserted + 1;
    END LOOP;
    RETURN rows_inserted;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION AddSearchRequest(
    SearchIndexId uuid,
    Date date
) RETURNS int AS $$
DECLARE
    rows_affected int := 0;
BEGIN
    -- Upsert logic: increment count if exists, else insert
    UPDATE dbo.SearchIndexRequestLog
    SET Count = Count + 1,
        ModifiedDate = CURRENT_TIMESTAMP
    WHERE SearchIndexId = SearchIndexId
      AND Date = Date;

    GET DIAGNOSTICS rows_affected = ROW_COUNT;

    IF rows_affected = 0 THEN
        INSERT INTO dbo.SearchIndexRequestLog (SearchIndexId, Count, Date, ModifiedDate)
        VALUES (SearchIndexId, 1, Date, CURRENT_TIMESTAMP);
        rows_affected := 1;
    END IF;

    RETURN rows_affected;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION AddFeedCredentials(
    SearchIndexId uuid,
    Username TEXT,
    PasswordHash TEXT
) RETURNS int AS $$
DECLARE
    rows_inserted int := 0;
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
        SearchIndexId,
        Username,
        PasswordHash,
        CURRENT_TIMESTAMP
    );
    GET DIAGNOSTICS rows_inserted = ROW_COUNT;
    RETURN rows_inserted;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION DeleteFeedCredentials(
    SearchIndexId uuid,
    Username TEXT
) RETURNS int AS $$
DECLARE
    rows_deleted int := 0;
BEGIN
    DELETE FROM dbo.FeedCredentials
    WHERE SearchIndexId = SearchIndexId
      AND Username = Username;
    GET DIAGNOSTICS rows_deleted = ROW_COUNT;
    RETURN rows_deleted;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetFeedCredentials(
    SearchIndexId uuid,
    Username TEXT
) RETURNS TABLE (
    SearchIndexId uuid,
    Username text,
    CreatedDate timestamp
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        SearchIndexId,
        Username,
        CreatedDate
    FROM dbo.FeedCredentials
    WHERE SearchIndexId = SearchIndexId
      AND Username = Username;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION UpdateFeedCredentials(
    SearchIndexId uuid,
    Username TEXT,
    PasswordHash TEXT
) RETURNS int AS $$
DECLARE
    rows_updated int := 0;
BEGIN
    UPDATE dbo.FeedCredentials
    SET PasswordHash = PasswordHash,
        ModifiedDate = CURRENT_TIMESTAMP
    WHERE SearchIndexId = SearchIndexId
      AND Username = Username;
    GET DIAGNOSTICS rows_updated = ROW_COUNT;
    RETURN rows_updated;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION AddFeed(
    SearchIndexId uuid,
    FeedType TEXT,
    FeedCron TEXT
) RETURNS int AS $$
DECLARE
    rows_inserted int := 0;
BEGIN
    -- Supersede previous latest feed
    PERFORM SupersedeLatestFeed(SearchIndexId);

    -- Insert new feed
    INSERT INTO dbo.Feeds
    (
        SearchIndexId,
        FeedType,
        FeedScheduleCron,
        CreatedDate,
        IsLatest
    )
    VALUES
    (
        SearchIndexId,
        FeedType,
        FeedCron,
        CURRENT_TIMESTAMP,
        1
    );
    GET DIAGNOSTICS rows_inserted = ROW_COUNT;
    RETURN rows_inserted;
END;
$$ LANGUAGE plpgsql;