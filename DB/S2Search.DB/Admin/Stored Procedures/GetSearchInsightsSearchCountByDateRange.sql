CREATE PROCEDURE [Admin].[GetSearchInsightsSearchCountByDateRange]
(
	@SearchIndexId uniqueidentifier,
	@DateFrom datetime,
	@DateTo datetime
)
AS

BEGIN

select 
d.[Date],
d.[Count]
from insights.SearchIndexRequestLog d
where d.SearchIndexId = @SearchIndexId
and d.[Date] between @DateFrom and @DateTo

END