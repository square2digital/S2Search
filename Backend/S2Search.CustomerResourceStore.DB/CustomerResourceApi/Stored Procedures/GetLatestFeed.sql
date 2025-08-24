CREATE PROCEDURE [CustomerResourceApi].[GetLatestFeed]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

SELECT TOP 1
f.FeedId,
f.SearchIndexId,
f.FeedType as [Type],
f.FeedScheduleCron as ScheduleCron,
f.CreatedDate,
f.SupersededDate,
f.IsLatest
FROM dbo.Feeds f
WHERE f.SearchIndexId = @SearchIndexId
AND f.IsLatest = 1

END