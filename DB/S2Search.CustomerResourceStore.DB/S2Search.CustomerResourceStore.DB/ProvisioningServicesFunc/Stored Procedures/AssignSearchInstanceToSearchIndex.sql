CREATE PROCEDURE [ProvisioningServicesFunc].[AssignSearchInstanceToSearchIndex]
(
	@SearchIndexId uniqueidentifier,
	@SearchInstanceId uniqueidentifier
)
AS

BEGIN
	
	UPDATE dbo.SearchIndex
	SET SearchInstanceId = @SearchInstanceId
	WHERE SearchIndexId = @SearchIndexId

END