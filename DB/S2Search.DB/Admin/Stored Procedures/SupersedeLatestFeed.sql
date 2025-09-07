CREATE PROCEDURE [Admin].[SupersedeLatestFeed]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

	UPDATE dbo.Feeds
	SET IsLatest = 0,
		SupersededDate = GETUTCDATE()
	WHERE SearchIndexId = @SearchIndexId
	AND IsLatest = 1
	
END