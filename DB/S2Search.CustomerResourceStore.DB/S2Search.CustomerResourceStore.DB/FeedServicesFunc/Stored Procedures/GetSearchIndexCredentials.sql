CREATE PROCEDURE [FeedServicesFunc].[GetSearchIndexCredentials]
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

END