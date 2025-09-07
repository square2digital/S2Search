CREATE PROCEDURE [SearchInsightsFunc].[AddDataPoints]
(
	@SearchIndexId uniqueidentifier,
	@SearchInsightsData [SearchInsightsData] READONLY
)
AS

BEGIN
	DECLARE @UtcNow datetime = GETUTCDATE();

	MERGE [insights].[SearchInsightsData] WITH (SERIALIZABLE) as target
	USING @SearchInsightsData as source
	ON @searchIndexId = target.SearchIndexId
	AND source.[DataCategory] = target.[DataCategory]
	AND source.[DataPoint] = target.[DataPoint]
	AND source.[Date] = target.[Date]
	WHEN MATCHED THEN
	UPDATE SET target.[Count] = target.[Count] + 1,
				target.[ModifiedDate] = @UtcNow
	WHEN NOT MATCHED THEN
	INSERT ([SearchIndexId], [DataCategory], [DataPoint], [Count], [Date], [ModifiedDate])
	VALUES (@SearchIndexId, source.[DataCategory], source.[DataPoint], 1, source.[Date], @UtcNow);

END