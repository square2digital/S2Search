CREATE PROCEDURE [CustomerResourceApi].[GetSearchInsightsByDataCategories]
(
	@SearchIndexId uniqueidentifier,
	@DateFrom datetime,
	@DateTo datetime,
	@DataCategories varchar(1000)
)
AS

BEGIN

select 
d.[DataCategory],
d.[DataPoint],
d.[Date],
d.[Count]
from insights.SearchInsightsData d
cross apply string_split(@DataCategories, ',') categories
where d.SearchIndexId = @SearchIndexId
and d.[Date] >= @DateFrom
and d.[Date] <= @DateTo
and categories.value = d.DataCategory

END