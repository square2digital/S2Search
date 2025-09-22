CREATE PROCEDURE [SearchInsightsFunc].[AddSearchRequest]
(
	@SearchIndexId uniqueidentifier,
	@Date date
)
AS

BEGIN
	DECLARE @UtcNow datetime = GETUTCDATE();

	MERGE [SearchIndexRequestLog] WITH (SERIALIZABLE) as target
	USING (	SELECT @SearchIndexId as [SearchIndexId], @Date as [Date]) as source
	ON source.[SearchIndexId] = target.[SearchIndexId]
	AND source.[Date] = target.[Date]
	WHEN MATCHED THEN
	UPDATE SET	target.[Count] = target.[Count] + 1, 
				target.[ModifiedDate] = @UtcNow
	WHEN NOT MATCHED THEN
	INSERT ([SearchIndexId], [Count], [Date], [ModifiedDate])
	VALUES (@SearchIndexId, 1, @Date, @UtcNow);

END