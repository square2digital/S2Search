CREATE PROCEDURE [ProvisioningServicesFunc].[GetAvailableSubscriptionForPricingTier]
(
	@PricingTier varchar(50)
)
AS

BEGIN

SELECT TOP 1
s.SubscriptionId,
s.Name
FROM dbo.Subscriptions s
INNER JOIN dbo.SubscriptionCapacity sc on sc.SubscriptionId = s.SubscriptionId
INNER JOIN dbo.SubscriptionResourceCapacity src on src.SubscriptionId = s.SubscriptionId
WHERE sc.ResourceGroupsAvailable > 0
AND src.PricingTier = @PricingTier
AND src.Available > 0

END