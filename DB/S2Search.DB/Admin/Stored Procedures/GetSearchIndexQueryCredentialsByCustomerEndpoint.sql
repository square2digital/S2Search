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