CREATE PROCEDURE [ProvisioningServicesFunc].[AddSearchIndexKey]
(
	@SearchIndexId uniqueidentifier,
	@Name varchar(100),
	@SearchInstanceKeyId uniqueidentifier
)
AS

BEGIN

	INSERT INTO dbo.SearchIndexKeys
	(
		SearchIndexId,
		[Name],
		SearchInstanceKeyId
	)
	VALUES
	(
		@SearchIndexId,
		@Name,
		@SearchInstanceKeyId
	)

END