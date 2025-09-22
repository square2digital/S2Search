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