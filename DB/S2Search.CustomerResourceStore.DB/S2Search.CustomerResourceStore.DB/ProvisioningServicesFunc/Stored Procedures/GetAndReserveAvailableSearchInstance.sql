CREATE PROCEDURE [ProvisioningServicesFunc].[GetAndReserveAvailableSearchInstance]
(
	@SearchIndexId uniqueidentifier,
	@ExpiryDate datetime,
	@Location varchar(50),
	@PricingTier varchar(50),
	@IsShared BIT
)
AS

BEGIN
	BEGIN TRAN

	declare @SearchInstanceId uniqueidentifier
	declare @SearchInstance table (
		SearchInstanceId uniqueidentifier,
		ServiceName varchar(255),
		SubscriptionId uniqueidentifier,
		ResourceGroup varchar(255),
		Location varchar(50),
		PricingTier varchar(50),
		IsShared bit
	)
	
	INSERT INTO @SearchInstance
	(
		SearchInstanceId,
		ServiceName,
		SubscriptionId,
		ResourceGroup,
		Location,
		PricingTier,
		IsShared
	)
	SELECT TOP 1
	sr.SearchInstanceId,
	sr.ServiceName,
	sr.SubscriptionId,
	sr.ResourceGroup,
	sr.Location,
	sr.PricingTier,
	sr.IsShared
	FROM dbo.SearchInstances sr
	INNER JOIN dbo.SearchInstanceCapacity src WITH (ROWLOCK, READPAST, UPDLOCK) on src.SearchInstanceId = sr.SearchInstanceId 
	WHERE sr.Location = @Location
	AND sr.PricingTier = @PricingTier
	AND sr.IsShared = 1
	AND src.IndexesAvailable > 0

	SET @SearchInstanceId = (SELECT SearchInstanceId FROM @SearchInstance)
	IF @SearchInstanceId IS NOT NULL
	BEGIN
		INSERT INTO dbo.SearchInstanceReservations
		(
			SearchInstanceId,
			SearchIndexId,
			ReservedDate,
			ExpiryDate
		)
		VALUES
		(
			@SearchInstanceId,
			@SearchIndexId,
			GETUTCDATE(),
			@ExpiryDate
		)

		UPDATE dbo.SearchInstanceCapacity WITH (ROWLOCK, READPAST, UPDLOCK)
		SET IndexesReserved = IndexesReserved + 1,
			IndexesAvailable = IndexesAvailable - 1
		WHERE SearchInstanceId = @SearchInstanceId
	END

	SELECT 
	SearchInstanceId,
	ServiceName as [Name],
	SubscriptionId,
	ResourceGroup,
	Location,
	PricingTier,
	IsShared
	FROM @SearchInstance

	COMMIT TRAN

END