﻿CREATE PROCEDURE [CustomerResourceApi].[GetSearchIndexQueryCredentialsByCustomerEndpoint]
(
	@CustomerEndpoint varchar(250)
)
AS

BEGIN

SELECT
si.SearchIndexId,
LOWER(si.IndexName) as [SearchIndexName],
i.ServiceName as [SearchInstanceName],
i.[Endpoint] as SearchInstanceEndpoint,
ik.ApiKey
FROM dbo.SearchIndex si
INNER JOIN dbo.SearchInterfaces sui on sui.SearchIndexId = si.SearchIndexId AND sui.IsLatest = 1
INNER JOIN dbo.SearchInstances i on i.SearchInstanceId = si.SearchInstanceId
INNER JOIN dbo.SearchInstanceKeys ik on ik.SearchInstanceId = i.SearchInstanceId 
									AND ik.KeyType = 'Query' 
									AND ik.Name = 'Query key' 
									AND ik.IsLatest = 1
WHERE sui.SearchEndpoint = @CustomerEndpoint

END