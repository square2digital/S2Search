CREATE PROCEDURE [FeedServicesFunc].[GetSearchIndexFeedProcessingData]
(
	@CustomerId uniqueidentifier,
	@SearchIndexName varchar(60)
)
AS

BEGIN

SELECT
si.SearchIndexId,
LOWER(si.IndexName) as [SearchIndexName],
i.SearchInstanceId,
i.ServiceName as [SearchInstanceName],
i.[Endpoint],
ik.ApiKey
FROM dbo.SearchIndex si
INNER JOIN dbo.SearchInstances i on i.SearchInstanceId = si.SearchInstanceId
INNER JOIN dbo.SearchInstanceKeys ik on ik.SearchInstanceId = i.SearchInstanceId 
									AND ik.KeyType = 'Admin' 
									AND ik.Name = 'Primary Admin key' 
									AND ik.IsLatest = 1
WHERE si.CustomerId = @CustomerId
AND si.IndexName = @SearchIndexName

SELECT TOP 1
f.DataFormat as FeedDataFormat,
s.SearchEndpoint
FROM dbo.SearchIndex si
INNER JOIN dbo.Feeds f on f.SearchIndexId = si.SearchIndexId AND f.IsLatest = 1
INNER JOIN dbo.SearchInterfaces s on s.SearchIndexId = si.SearchIndexId AND s.IsLatest = 1
WHERE si.CustomerId = @CustomerId
AND si.IndexName = @SearchIndexName

END