CREATE PROCEDURE [CustomerResourceApi].[SupersedeLatestSearchInterface]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

	UPDATE dbo.SearchInterfaces
	SET IsLatest = 0,
		SupersededDate = GETUTCDATE()
	WHERE SearchIndexId = @SearchIndexId
	AND IsLatest = 1
	
END