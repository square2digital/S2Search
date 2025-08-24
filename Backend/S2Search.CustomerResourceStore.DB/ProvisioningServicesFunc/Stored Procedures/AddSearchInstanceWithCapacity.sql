CREATE PROCEDURE [ProvisioningServicesFunc].[AddSearchInstanceWithCapacity]
(
	@SearchInstanceWithCapacity [SearchInstanceWithCapacity] READONLY
)
AS

BEGIN

	INSERT INTO dbo.SearchInstances
	(
		SearchInstanceId,
		ServiceName,
		SubscriptionId,
		ResourceGroup,
		[Location],
		PricingTier,
		Replicas,
		[Partitions],
		IsShared
	)
	SELECT
	SearchInstanceId,
	ServiceName,
	SubscriptionId,
	ResourceGroup,
	[Location],
	PricingTier,
	Replicas,
	[Partitions],
	IsShared
	FROM @SearchInstanceWithCapacity

	INSERT INTO dbo.SearchInstanceCapacity
	(
		SearchInstanceId,
		StorageQuotaMB,
		StorageUsedMB,
		IndexesQuota,
		IndexesUsed,
		IndexesAvailable,
		IndexesReserved,
		DocumentsQuota,
		DocumentsUsed,
		ModifiedDate
	)
	SELECT 
	SearchInstanceId,
	StorageQuotaMB,
	StorageUsedMB,
	IndexesQuota,
	IndexesUsed,
	IndexesAvailable,
	IndexesReserved,
	DocumentsQuota,
	DocumentsUsed,
	GETUTCDATE()
	FROM @SearchInstanceWithCapacity

END