CREATE PROCEDURE [ProvisioningServicesFunc].[UpdateSearchInstanceCapacity]
(
	@SearchInstanceId uniqueidentifier
)
AS

BEGIN

	UPDATE dbo.SearchInstanceCapacity
	SET IndexesUsed = IndexesUsed + 1,
		IndexesReserved = IndexesReserved - 1,
		ModifiedDate = GETUTCDATE()
	WHERE SearchInstanceId = @SearchInstanceId

END