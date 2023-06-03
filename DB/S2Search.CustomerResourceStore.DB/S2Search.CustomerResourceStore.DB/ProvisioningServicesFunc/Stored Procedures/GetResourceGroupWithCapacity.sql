CREATE PROCEDURE [ProvisioningServicesFunc].[GetResourceGroupWithCapacity]
(
	@ResourceGroup varchar(255)
)
AS

BEGIN

	SELECT
	rg.ResourceGroup as [Name],
	rg.SubscriptionId,
	rgc.ResourcesQuota,
	rgc.ResourcesUsed,
	rgc.ResourcesAvailable
	FROM dbo.ResourceGroups rg
	INNER JOIN dbo.ResourceGroupsCapacity rgc on rgc.ResourceGroup = rg.ResourceGroup
	WHERE rg.ResourceGroup = @ResourceGroup

END