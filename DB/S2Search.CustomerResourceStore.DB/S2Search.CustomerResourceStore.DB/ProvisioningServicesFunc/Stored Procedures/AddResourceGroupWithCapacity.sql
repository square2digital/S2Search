CREATE PROCEDURE [ProvisioningServicesFunc].[AddResourceGroupWithCapacity]
(
	@ResourceGroup varchar(255),
	@SubscriptionId uniqueidentifier,
	@ResourcesQuota INT,
	@ResourcesAvailable INT
)
AS

BEGIN

	INSERT INTO dbo.ResourceGroups
	(
		ResourceGroup,
		SubscriptionId
	)
	VALUES
	(
		@ResourceGroup,
		@SubscriptionId
	)

	INSERT INTO dbo.ResourceGroupsCapacity
	(
		ResourceGroup,
		ResourcesQuota,
		ResourcesUsed,
		ResourcesAvailable,
		ModifiedDate
	)
	VALUES
	(
		@ResourceGroup,
		@ResourcesQuota,
		1,
		@ResourcesAvailable,
		GETUTCDATE()
	)

END