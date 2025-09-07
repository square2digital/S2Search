CREATE PROCEDURE [Admin].[GetSearchIndex]
(
	@SearchIndexId uniqueidentifier,
	@CustomerId uniqueidentifier
)
AS

BEGIN

SELECT
[search].SearchIndexId,
[search].CustomerId,
[search].IndexName,
[search].FriendlyName,
[service].[Endpoint],
[service].PricingTier,
[search].CreatedDate,
[service].SearchInstanceId,
[service].ServiceName,
[service].SubscriptionId,
[service].ResourceGroup,
[service].[Location],
[service].PricingTier,
[service].Replicas,
[service].[Partitions],
[service].IsShared
FROM dbo.SearchIndex [search]
LEFT OUTER JOIN dbo.SearchInstances [service] on [service].SearchInstanceId = search.SearchInstanceId
WHERE search.SearchIndexId = @SearchIndexId
and search.CustomerId = @CustomerId

END