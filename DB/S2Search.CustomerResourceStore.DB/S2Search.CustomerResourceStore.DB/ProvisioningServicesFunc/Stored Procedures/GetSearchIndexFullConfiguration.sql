CREATE PROCEDURE [ProvisioningServicesFunc].[GetSearchIndexFullConfiguration]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

DECLARE @SearchInstanceId uniqueidentifier = (SELECT TOP 1 SearchIndexId FROM dbo.SearchIndex WHERE SearchIndexId = @SearchIndexId) 

SELECT
[SearchIndexId]
,[IndexName]
,[CreatedDate]
FROM dbo.SearchIndex searchRes
WHERE searchRes.SearchIndexId = @SearchIndexId

SELECT
sr.SearchInstanceId,
sr.ServiceName as [Name],
sr.SubscriptionId,
sr.ResourceGroup,
sr.[Location],
sr.PricingTier,
sr.Replicas,
sr.[Partitions],
sr.IsShared
FROM dbo.SearchInstances sr
WHERE sr.SearchInstanceId = @SearchInstanceId

SELECT 
FeedId,
FeedType as [Type],
FeedScheduleCron as ScheduleCron
FROM dbo.Feeds 
where SearchIndexId = @SearchIndexId 
and IsLatest = 1

SELECT 
NotificationRuleId,
TransmitType,
Recipients,
[Trigger]
FROM dbo.NotificationRules 
where SearchIndexId = @SearchIndexId 
and IsLatest = 1

SELECT 
SynonymId,
KeyWord as [Key],
SolrFormat
FROM dbo.[Synonyms]
where SearchIndexId = @SearchIndexId 
and IsLatest = 1

SELECT 
SearchInterfaceId,
InterfaceType as [Type],
LogoURL,
BannerStyle
FROM dbo.SearchInterfaces 
where SearchIndexId = @SearchIndexId 
and IsLatest = 1

END