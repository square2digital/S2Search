CREATE PROCEDURE [CustomerResourceApi].[GetSynonyms]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

SELECT
SynonymId,
SearchIndexId,
KeyWord as [Key],
SolrFormat,
CreatedDate
FROM [dbo].[Synonyms]
WHERE SearchIndexId = @SearchIndexId
AND IsLatest = 1

END