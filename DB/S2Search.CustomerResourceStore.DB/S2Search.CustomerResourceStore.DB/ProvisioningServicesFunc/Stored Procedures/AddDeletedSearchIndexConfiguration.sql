CREATE PROCEDURE [ProvisioningServicesFunc].[AddDeletedSearchIndexConfiguration]
(
	@CustomerId	uniqueidentifier,
	@SearchIndexId uniqueidentifier,
	@Configuration varchar(max)
)
AS

BEGIN
	
	INSERT INTO dbo.DeletedSearchIndexConfiguration
	(
		CustomerId,
		SearchIndexId,
		[Configuration]
	)
	VALUES
	(
		@CustomerId,
		@SearchIndexId,
		@Configuration
	)

END