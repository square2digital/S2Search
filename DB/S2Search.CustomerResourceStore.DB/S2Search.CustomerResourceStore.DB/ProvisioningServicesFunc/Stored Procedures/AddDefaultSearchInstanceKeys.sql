CREATE PROCEDURE [ProvisioningServicesFunc].[AddDefaultSearchInstanceKeys]
(
	@SearchInstanceId uniqueidentifier,
	@PrimaryAdminKey varchar(255),
	@SecondaryAdminKey varchar(255),
	@DefaultQueryKey varchar(255)
)
AS

BEGIN

	IF ISNULL(@PrimaryAdminKey, '') != ''
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
			NewId(),	
			@SearchInstanceId,
			'Admin',
			'PrimaryAdminKey',
			@PrimaryAdminKey
		)
	END

	IF ISNULL(@SecondaryAdminKey, '') != ''
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
			NewId(),
			@SearchInstanceId,
			'Admin',
			'SecondaryAdminKey',
			@SecondaryAdminKey
		)
	END

	IF ISNULL(@DefaultQueryKey, '') != ''
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
			NewId(),
			@SearchInstanceId,
			'Query',
			'DefaultQueryKey',
			@DefaultQueryKey
		)
	END

END