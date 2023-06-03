CREATE PROCEDURE [ProvisioningServicesFunc].[DeleteSearchIndex]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN
	
	DELETE FROM dbo.SearchInterfaces
	WHERE SearchIndexId = @SearchIndexId

	DELETE FROM dbo.NotificationRules
	WHERE SearchIndexId = @SearchIndexId

	DELETE FROM dbo.[Synonyms]
	WHERE SearchIndexId = @SearchIndexId

	DELETE FROM dbo.Feeds
	WHERE SearchIndexId = @SearchIndexId

	DELETE FROM dbo.SearchIndex
	WHERE SearchIndexId = @SearchIndexId

END