CREATE PROCEDURE [CustomerResourceApi].[GetSynonymById]
(
	@SearchIndexId uniqueidentifier,
	@SynonymId uniqueidentifier
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
AND SynonymId = @SynonymId
AND IsLatest = 1

END