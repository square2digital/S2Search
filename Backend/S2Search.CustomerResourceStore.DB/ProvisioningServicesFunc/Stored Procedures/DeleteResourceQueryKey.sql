CREATE PROCEDURE [ProvisioningServicesFunc].[DeleteResourceQueryKey]
(
	@SearchIndexId uniqueidentifier,
	@SearchInstanceKeyId uniqueidentifier
)
AS

BEGIN

	DELETE FROM dbo.SearchIndexKeys
	WHERE SearchIndexId = @SearchIndexId
	AND SearchInstanceKeyId = @SearchInstanceKeyId

	DELETE FROM dbo.SearchInstanceKeys
	WHERE SearchInstanceKeyId = @SearchInstanceKeyId

END