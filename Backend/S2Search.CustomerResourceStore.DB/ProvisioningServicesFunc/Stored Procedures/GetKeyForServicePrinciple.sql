CREATE PROCEDURE [ProvisioningServicesFunc].[GetKeyForServicePrinciple]
(
	@ClientId uniqueidentifier
)
AS

BEGIN

SELECT TOP 1
[Value]
FROM dbo.ClientKeys ck
WHERE ck.ClientId = @ClientId
AND IsLatest = 1

END