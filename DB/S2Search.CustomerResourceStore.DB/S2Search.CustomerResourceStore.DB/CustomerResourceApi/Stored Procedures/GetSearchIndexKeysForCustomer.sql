CREATE PROCEDURE [CustomerResourceApi].[GetSearchIndexKeysForCustomer]
(
	@SearchIndexId uniqueidentifier,
	@CustomerId uniqueidentifier
)
AS

BEGIN

SELECT
srk.[Name],
srk.SearchInstanceKeyId as [ApiKey],
srk.CreatedDate
FROM dbo.SearchIndexKeys srk
INNER JOIN dbo.SearchIndex si on si.SearchIndexId = srk.SearchIndexId
WHERE srk.SearchIndexId = @SearchIndexId
AND si.CustomerId = @CustomerId

END