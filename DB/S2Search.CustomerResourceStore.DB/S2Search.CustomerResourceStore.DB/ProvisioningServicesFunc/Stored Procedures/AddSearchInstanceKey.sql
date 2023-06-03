CREATE PROCEDURE [ProvisioningServicesFunc].[AddSearchInstanceKey]
(
	@SearchInstanceKeyId uniqueidentifier,
	@SearchInstanceId uniqueidentifier,
	@KeyType varchar(50),
	@KeyName varchar(100),
	@ApiKey varchar(255)
)
AS

BEGIN

	INSERT INTO dbo.SearchInstanceKeys
	(
		SearchInstanceKeyId,
		SearchInstanceId,
		KeyType,
		[Name],
		ApiKey
	)
	VALUES
	(
		@SearchInstanceKeyId,	
		@SearchInstanceId,
		@KeyType,
		@KeyName,
		@ApiKey
	)

END