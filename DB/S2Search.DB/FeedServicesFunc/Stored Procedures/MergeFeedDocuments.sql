CREATE PROCEDURE [FeedServicesFunc].[MergeFeedDocuments]
(
	@SearchIndexId uniqueidentifier,
	@NewFeedDocuments [NewFeedDocuments] READONLY
)
AS

BEGIN
	DECLARE @UtcNow datetime = GETUTCDATE();

	MERGE [dbo].[FeedCurrentDocuments] WITH (SERIALIZABLE) as target
	USING @NewFeedDocuments as source
	ON @searchIndexId = target.SearchIndexId
	AND source.DocumentId = target.Id
	WHEN MATCHED THEN
	UPDATE SET target.[CreatedDate] = @UtcNow
	WHEN NOT MATCHED BY target THEN
	INSERT ([Id], [SearchIndexId])
	VALUES (source.[DocumentId], @SearchIndexId)
	WHEN NOT MATCHED BY source AND target.SearchIndexId = @SearchIndexId THEN
	DELETE;

END