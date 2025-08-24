CREATE PROCEDURE [FeedServicesFunc].[GetFeedDataFormat]
(
	@CustomerId uniqueidentifier,
	@SearchIndexName varchar(60)
)
AS

BEGIN

SELECT TOP 1
f.DataFormat
FROM dbo.SearchIndex si
INNER JOIN dbo.Feeds f on f.SearchIndexId = si.SearchIndexId AND f.IsLatest = 1
WHERE si.CustomerId = @CustomerId
AND si.IndexName = @SearchIndexName

END