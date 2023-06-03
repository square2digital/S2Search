CREATE PROCEDURE [CustomerResourceApi].[AddFeed]
(
	@SearchIndexId uniqueidentifier,
	@FeedType varchar(20),
	@FeedCron varchar(255)
)
AS

BEGIN

	EXEC [CustomerResourceApi].[SupersedeLatestFeed] @SearchIndexId = @SearchIndexId
	
	INSERT INTO dbo.Feeds
	(
		SearchIndexId,
		FeedType,
		FeedScheduleCron
	)
	VALUES
	(
		@SearchIndexId,
		@FeedType,
		@FeedCron
	)

END