CREATE PROCEDURE [Admin].[GetSearchInsightsByDataCategories]
(
	@SearchIndexId uniqueidentifier,
	@DateFrom datetime,
	@DateTo datetime,
	@DataCategories varchar(1000)
)
AS

BEGIN

SELECT 
d.[DataCategory],
d.[DataPoint],
d.[Date],
d.[Count]
FROM insights.SearchInsightsData d
CROSS APPLY string_split(@DataCategories, ',') categories
WHERE d.SearchIndexId = @SearchIndexId
AND d.[Date] >= @DateFrom
AND d.[Date] <= @DateTo
AND categories.value = d.DataCategory

END