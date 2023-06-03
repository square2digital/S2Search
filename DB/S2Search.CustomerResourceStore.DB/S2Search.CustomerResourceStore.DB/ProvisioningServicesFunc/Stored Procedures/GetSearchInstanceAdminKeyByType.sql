CREATE PROCEDURE [ProvisioningServicesFunc].[GetSearchInstanceAdminKeyByType]
(
	@SearchInstanceId uniqueidentifier,
	@KeyType varchar(20)
)
AS

BEGIN

SELECT TOP 1
ApiKey
FROM dbo.SearchInstanceKeys srk
WHERE srk.SearchInstanceId = @SearchInstanceId
AND KeyType = @KeyType
AND IsLatest = 1

END