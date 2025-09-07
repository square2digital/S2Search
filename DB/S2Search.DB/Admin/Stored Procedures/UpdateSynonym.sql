CREATE PROCEDURE [Admin].[UpdateSynonym]
(
	@SearchIndexId uniqueidentifier,
	@SynonymId uniqueidentifier,
	@KeyWord varchar(50),
	@SolrFormat varchar(max)
)
AS

BEGIN

	UPDATE [dbo].[Synonyms]
	SET	[KeyWord] = @KeyWord,
	[SolrFormat] = @SolrFormat
	WHERE SearchIndexId = @SearchIndexId
	AND SynonymId = @SynonymId
	AND IsLatest = 1
	
END