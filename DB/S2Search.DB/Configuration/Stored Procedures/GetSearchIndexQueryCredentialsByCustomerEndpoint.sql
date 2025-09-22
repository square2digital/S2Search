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