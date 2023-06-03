CREATE PROCEDURE [ProvisioningServicesFunc].[DeleteResourceGroup]
(
	@ResourceGroup varchar(255)
)
AS

BEGIN
	
	BEGIN TRAN

	DELETE FROM dbo.ResourceGroupsCapacity
	WHERE ResourceGroup = @ResourceGroup

	DELETE FROM dbo.ResourceGroups
	WHERE ResourceGroup = @ResourceGroup

	IF @@ERROR > 0
	BEGIN
		ROLLBACK TRAN
	END

	COMMIT TRAN

END