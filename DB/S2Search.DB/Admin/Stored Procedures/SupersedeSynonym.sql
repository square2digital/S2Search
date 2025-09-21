CREATE PROCEDURE [Admin].[SupersedeSynonym]
(
	@SearchIndexId uniqueidentifier,
	@SynonymId uniqueidentifier
)
AS

BEGIN

	UPDATE [dbo].[Synonyms]
	SET IsLatest = 0,
		SupersededDate = GETUTCDATE()
	WHERE SearchIndexId = @SearchIndexId
	AND Id = @SynonymId
	AND IsLatest = 1
	
END