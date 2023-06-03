CREATE PROCEDURE [ProvisioningServicesFunc].[GetSubscriptionById]
(
	@SubscriptionId uniqueidentifier
)
AS

BEGIN

SELECT TOP 1
s.SubscriptionId,
s.Name
FROM dbo.Subscriptions s
WHERE s.SubscriptionId = @SubscriptionId

END