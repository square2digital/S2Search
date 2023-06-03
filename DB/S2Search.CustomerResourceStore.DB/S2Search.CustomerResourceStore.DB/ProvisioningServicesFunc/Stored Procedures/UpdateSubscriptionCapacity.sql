CREATE PROCEDURE [ProvisioningServicesFunc].[UpdateSubscriptionCapacity]
(
	@SubscriptionId uniqueidentifier
)
AS

BEGIN

	UPDATE dbo.SubscriptionCapacity
	SET ResourceGroupsUsed = ResourceGroupsUsed + 1,
		ResourceGroupsAvailable = ResourceGroupsAvailable - 1,
		ModifiedDate = GETUTCDATE()
	WHERE SubscriptionId = @SubscriptionId

END