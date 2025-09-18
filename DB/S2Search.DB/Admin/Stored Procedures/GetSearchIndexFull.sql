CREATE PROCEDURE [Admin].[GetSearchIndexFull]
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
AND search.CustomerId = @CustomerId

--If no results it didnt match on SearchIndexId and CustomerId so override the SearchIndexId so that the other selects do not return a result
IF @@ROWCOUNT = 0
BEGIN
	SET @SearchIndexId = NULL
END

SELECT 
FeedId,
SearchIndexId,
FeedType as [Type],
FeedScheduleCron as ScheduleCron,
CreatedDate,
SupersededDate,
IsLatest
FROM dbo.Feeds 
WHERE SearchIndexId = @SearchIndexId 
AND IsLatest = 1


SELECT 
NotificationRuleId,
TransmitType,
Recipients,
[Trigger]
FROM dbo.NotificationRules 
WHERE SearchIndexId = @SearchIndexId 
AND IsLatest = 1

SELECT 
SynonymId,
KeyWord as [Key],
SolrFormat
FROM dbo.[Synonyms]
WHERE SearchIndexId = @SearchIndexId 
AND IsLatest = 1

SELECT 
SearchInterfaceId,
InterfaceType as [Type],
LogoURL,
BannerStyle
FROM dbo.SearchInterfaces 
WHERE SearchIndexId = @SearchIndexId 
AND IsLatest = 1

END
GO