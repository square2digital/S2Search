CREATE PROCEDURE [CustomerResourceApi].[AddSynonym]
(
	@SynonymId uniqueidentifier,
	@SearchIndexId uniqueidentifier,
	@KeyWord varchar(50),
	@SolrFormat varchar(max)
)
AS

BEGIN

	INSERT INTO [dbo].[Synonyms]
	(
		SynonymId,
		SearchIndexId,
		KeyWord,
		SolrFormat
	)
	VALUES
	(
		@SynonymId,
		@SearchIndexId,
		@KeyWord,
		@SolrFormat
	)

	SELECT @SynonymId as SynonymId

END