CREATE PROCEDURE [ProvisioningServicesFunc].[GetServicePrincipleForSubscription]
(
	@SubscriptionId uniqueidentifier
)
AS

BEGIN

SELECT TOP 1
sp.ClientId,
sp.[Name],
sp.TenantId,
sp.SubscriptionId
FROM dbo.ServicePrinciples sp
WHERE sp.SubscriptionId = @SubscriptionId

END