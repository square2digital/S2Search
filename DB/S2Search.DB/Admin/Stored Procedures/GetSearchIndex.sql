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