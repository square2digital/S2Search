CREATE PROCEDURE [CustomerResourceApi].[GetDashboardSummaryForCustomer]
(
	@CustomerId uniqueidentifier,
	@StartDate datetime,
	@EndDate datetime
)
AS

BEGIN

SELECT 
si.SearchIndexId, 
si.FriendlyName as SearchIndexFriendlyName, 
si.CreatedDate as SearchIndexCreatedDate,
LatestFeedNotification.[Event] as NotificationEvent,
LatestFeedNotification.Category as NotificationCategory,
LatestFeedNotification.CreatedDate as NotificationCreatedDate,
SynonymsCount.[Count] as SynonymsCount,
NotificationsCount.[Count] as NotificationsCount
FROM dbo.SearchIndex si
OUTER APPLY (
	SELECT TOP 1 
	n.[Event],
	n.Category,
	n.TransmitType,
	n.CreatedDate
	FROM dbo.Notifications n
	WHERE SearchIndexId = si.SearchIndexId 
	AND Category = 'Feed' 
	ORDER BY CreatedDate DESC
) LatestFeedNotification
CROSS APPLY (
	SELECT 
	COUNT(SynonymId) as [Count]
	FROM dbo.[Synonyms] 
	WHERE SearchIndexId = si.SearchIndexId 
) SynonymsCount
	CROSS APPLY (
	SELECT 
	COUNT(NotificationId) as [Count]
	FROM dbo.Notifications 
	where SearchIndexId = si.SearchIndexId 
	AND CreatedDate BETWEEN @StartDate AND @EndDate
) NotificationsCount
WHERE si.CustomerId = @CustomerId

END