CREATE PROCEDURE [Admin].[GetSynonymById]
(
	@SearchIndexId uniqueidentifier,
	@SynonymId uniqueidentifier
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
AND Id = @SynonymId
AND IsLatest = 1

END