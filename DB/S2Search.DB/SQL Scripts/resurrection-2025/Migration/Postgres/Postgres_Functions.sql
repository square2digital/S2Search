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

-- ==============================
-- 1. GetLatestFeed
-- ==============================

CREATE OR REPLACE FUNCTION GetLatestFeed(
    p_SearchIndexId uuid
)
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
    FROM Feeds f
    WHERE f.SearchIndexId = p_SearchIndexId
      AND f.IsLatest = 1
    LIMIT 1;
END;
$$ LANGUAGE plpgsql;

-- ==============================
-- 2. GetSearchIndex
-- ==============================

CREATE OR REPLACE FUNCTION GetSearchIndex(
    p_SearchIndexId uuid,
    p_CustomerId uuid
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
    FROM SearchIndex search
    LEFT OUTER JOIN SearchInstances service ON service.Id = search.SearchInstanceId
    WHERE search.Id = p_SearchIndexId
      AND search.CustomerId = p_CustomerId;
END;
$$ LANGUAGE plpgsql;

-- ==============================
-- 3. GetSearchIndexByFriendlyName
-- ==============================

CREATE OR REPLACE FUNCTION GetSearchIndexByFriendlyName(
    p_CustomerId uuid,
    p_FriendlyName TEXT
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
    WHERE si.CustomerId = p_CustomerId
      AND si.FriendlyName = p_FriendlyName;
END;
$$ LANGUAGE plpgsql;

-- ==============================
-- 4. GetSearchIndexFull
-- ==============================

CREATE OR REPLACE FUNCTION GetSearchIndexFull(
    p_SearchIndexId uuid,
    p_CustomerId uuid
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
    WHERE search.Id = p_SearchIndexId
      AND search.CustomerId = p_CustomerId;
END;
$$ LANGUAGE plpgsql;

-- ==============================
-- 5. GetSynonymById
-- ==============================

CREATE OR REPLACE FUNCTION GetSynonymById(
    p_SearchIndexId uuid,
    p_SynonymId uuid
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
    WHERE SearchIndexId = p_SearchIndexId
      AND Id = p_SynonymId
      AND IsLatest = 1;
END;
$$ LANGUAGE plpgsql;

-- ==============================
-- 6. GetSynonymByKeyWord
-- ==============================

CREATE OR REPLACE FUNCTION GetSynonymByKeyWord(
    p_SearchIndexId uuid,
    p_KeyWord TEXT
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
    WHERE SearchIndexId = p_SearchIndexId
      AND KeyWord = p_KeyWord
      AND IsLatest = 1;
END;
$$ LANGUAGE plpgsql;

-- ==============================
-- 7. SupersedeLatestFeed
-- ==============================

CREATE OR REPLACE FUNCTION SupersedeLatestFeed(
    p_SearchIndexId uuid
) RETURNS int AS $$
DECLARE
    rows_updated int;
BEGIN
    UPDATE dbo.Feeds
    SET IsLatest = 0,
        SupersededDate = CURRENT_TIMESTAMP
    WHERE SearchIndexId = p_SearchIndexId
      AND IsLatest = 1;
    GET DIAGNOSTICS rows_updated = ROW_COUNT;
    RETURN rows_updated;
END;
$$ LANGUAGE plpgsql;

-- ==============================
-- 8. SupersedeSynonym
-- ==============================

CREATE OR REPLACE FUNCTION SupersedeSynonym(
    p_SearchIndexId uuid,
    p_SynonymId uuid
) RETURNS int AS $$
DECLARE
    rows_updated int;
BEGIN
    UPDATE dbo.Synonyms
    SET IsLatest = 0,
        SupersededDate = CURRENT_TIMESTAMP
    WHERE SearchIndexId = p_SearchIndexId
      AND Id = p_SynonymId
      AND IsLatest = 1;
    GET DIAGNOSTICS rows_updated = ROW_COUNT;
    RETURN rows_updated;
END;
$$ LANGUAGE plpgsql;

-- ==============================
-- 9. UpdateSynonym
-- ==============================

CREATE OR REPLACE FUNCTION UpdateSynonym(
    p_SearchIndexId uuid,
    p_SynonymId uuid,
    p_KeyWord TEXT,
    p_SolrFormat TEXT
) RETURNS int AS $$
DECLARE
    rows_updated int;
BEGIN
    UPDATE dbo.Synonyms
    SET KeyWord = p_KeyWord,
        SolrFormat = p_SolrFormat
    WHERE SearchIndexId = p_SearchIndexId
      AND Id = p_SynonymId
      AND IsLatest = 1;
    GET DIAGNOSTICS rows_updated = ROW_COUNT;
    RETURN rows_updated;
END;
$$ LANGUAGE plpgsql;

-- ==============================
-- 10. DeleteFeedCredentials
-- ==============================

CREATE OR REPLACE FUNCTION DeleteFeedCredentials(
    p_SearchIndexId uuid,
    p_Username TEXT
) RETURNS int AS $$
DECLARE
    rows_deleted int := 0;
BEGIN
    DELETE FROM dbo.FeedCredentials
    WHERE SearchIndexId = p_SearchIndexId
      AND Username = p_Username;
    GET DIAGNOSTICS rows_deleted = ROW_COUNT;
    RETURN rows_deleted;
END;
$$ LANGUAGE plpgsql;

-- ==============================
-- 11. GetFeedCredentials
-- ==============================

CREATE OR REPLACE FUNCTION GetFeedCredentials(
    p_SearchIndexId uuid,
    p_Username TEXT
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
    WHERE SearchIndexId = p_SearchIndexId
      AND Username = p_Username;
END;
$$ LANGUAGE plpgsql;

-- ==============================
-- 12. UpdateFeedCredentials
-- ==============================

CREATE OR REPLACE FUNCTION UpdateFeedCredentials(
    p_SearchIndexId uuid,
    p_Username TEXT,
    p_PasswordHash TEXT
) RETURNS int AS $$
DECLARE
    rows_updated int := 0;
BEGIN
    UPDATE dbo.FeedCredentials
    SET PasswordHash = p_PasswordHash,
        ModifiedDate = CURRENT_TIMESTAMP
    WHERE SearchIndexId = p_SearchIndexId
      AND Username = p_Username;
    GET DIAGNOSTICS rows_updated = ROW_COUNT;
    RETURN rows_updated;
END;
$$ LANGUAGE plpgsql;

-- ==============================
-- 13. AddFeed
-- ==============================

CREATE OR REPLACE FUNCTION AddFeed(
    p_SearchIndexId uuid,
    p_FeedType TEXT,
    p_FeedCron TEXT
) RETURNS int AS $$
DECLARE
    rows_inserted int := 0;
BEGIN
    -- Supersede previous latest feed
    PERFORM SupersedeLatestFeed(p_SearchIndexId);

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
        p_SearchIndexId,
        p_FeedType,
        p_FeedCron,
        CURRENT_TIMESTAMP,
        1
    );
    GET DIAGNOSTICS rows_inserted = ROW_COUNT;
    RETURN rows_inserted;
END;
$$ LANGUAGE plpgsql;