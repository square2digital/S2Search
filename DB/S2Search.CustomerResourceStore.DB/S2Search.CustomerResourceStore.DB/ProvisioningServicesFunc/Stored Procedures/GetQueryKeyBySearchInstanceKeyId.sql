CREATE PROCEDURE [ProvisioningServicesFunc].[GetQueryKeyBySearchInstanceKeyId]
(
	@SearchInstanceKeyId uniqueidentifier
)
AS

BEGIN

SELECT TOP 1
ApiKey
FROM dbo.SearchInstanceKeys srk
WHERE srk.SearchInstanceKeyId = @SearchInstanceKeyId
AND KeyType = 'Query'
AND IsLatest = 1

END