CREATE PROCEDURE [Admin].[GetSearchInsightsSearchCountByDateRange]
(
	@SearchIndexId uniqueidentifier,
	@DateFrom datetime,
	@DateTo datetime
)
AS

BEGIN

SELECT 
d.[Date],
d.[Count]
FROM dbo.SearchIndexRequestLog d
WHERE d.SearchIndexId = @SearchIndexId
AND d.[Date] BETWEEN @DateFrom AND @DateTo

END