CREATE PROCEDURE [Admin].[GetSynonyms]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

SELECT
Id,
SearchIndexId,
KeyWord as [Key],
SolrFormat,
CreatedDate
FROM [dbo].[Synonyms]
WHERE SearchIndexId = @SearchIndexId
AND IsLatest = 1

END