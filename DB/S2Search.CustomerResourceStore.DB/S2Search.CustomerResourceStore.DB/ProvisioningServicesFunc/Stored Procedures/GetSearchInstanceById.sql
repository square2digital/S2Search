CREATE PROCEDURE [ProvisioningServicesFunc].[GetSearchInstanceById]
(
	@SearchInstanceId uniqueidentifier
)
AS

BEGIN

SELECT
sr.SearchInstanceId,
sr.ServiceName as [Name],
sr.SubscriptionId,
sr.ResourceGroup,
sr.[Location],
sr.PricingTier,
sr.Replicas,
sr.[Partitions],
sr.IsShared
FROM dbo.SearchInstances sr
WHERE sr.SearchInstanceId = @SearchInstanceId

END