CREATE PROCEDURE [Admin].[GetSearchIndexFull]
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
LEFT OUTER JOIN dbo.SearchInstances [service] on [service].Id = search.Id
WHERE search.Id = @SearchIndexId
AND search.CustomerId = @CustomerId

--If no results it didnt match on SearchIndexId and CustomerId so override the SearchIndexId so that the other selects do not return a result
IF @@ROWCOUNT = 0
BEGIN
	SET @SearchIndexId = NULL
END

SELECT 
Id,
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
Id,
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