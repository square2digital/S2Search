CREATE PROCEDURE [FeedServicesFunc].[GetLatestGenericSynonymsByCategory]
(
	@Category varchar(50)
)
AS

BEGIN

SELECT
Id,
Category,
SolrFormat,
CreatedDate
FROM [dbo].[Synonyms]
WHERE Category = @Category
AND IsLatest = 1

END