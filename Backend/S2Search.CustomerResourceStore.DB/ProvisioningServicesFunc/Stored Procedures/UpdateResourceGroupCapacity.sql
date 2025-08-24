CREATE PROCEDURE [ProvisioningServicesFunc].[UpdateResourceGroupCapacity]
(
	@ResourceGroup varchar(255),
	@DecreaseCapacity BIT
)
AS

BEGIN

	DECLARE @Value INT = CASE WHEN @DecreaseCapacity = 0 THEN -1 ELSE 1 END;

	UPDATE dbo.ResourceGroupsCapacity
	SET ResourcesUsed = ResourcesUsed + @Value,
		ResourcesAvailable = ResourcesAvailable - @Value,
		ModifiedDate = GETUTCDATE()
	WHERE ResourceGroup = @ResourceGroup

END