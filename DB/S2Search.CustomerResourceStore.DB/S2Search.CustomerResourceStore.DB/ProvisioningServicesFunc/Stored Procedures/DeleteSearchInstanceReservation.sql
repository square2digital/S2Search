CREATE PROCEDURE [ProvisioningServicesFunc].[DeleteSearchInstanceReservation]
(
	@SearchInstanceId uniqueidentifier,
	@SearchIndexId uniqueidentifier
)
AS

BEGIN
	
	DELETE FROM dbo.SearchInstanceReservations
	WHERE SearchInstanceId = @SearchInstanceId
	AND SearchIndexId = @SearchIndexId

END