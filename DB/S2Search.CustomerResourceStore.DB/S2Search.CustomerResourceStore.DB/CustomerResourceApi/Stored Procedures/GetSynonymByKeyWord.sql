CREATE PROCEDURE [CustomerResourceApi].[GetSynonymByKeyWord]
(
	@SearchIndexId uniqueidentifier,
	@KeyWord varchar(30)
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
AND KeyWord = @KeyWord
AND IsLatest = 1

END