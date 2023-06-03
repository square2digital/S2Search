CREATE PROCEDURE [CustomerResourceApi].[GetSearchIndexByFriendlyName]
(
	@CustomerId uniqueidentifier,
	@FriendlyName varchar(100)
)
AS

BEGIN

SELECT
si.SearchIndexId,
si.SearchInstanceId,
si.CustomerId,
si.FriendlyName,
si.IndexName
FROM dbo.SearchIndex si
WHERE si.CustomerId = @CustomerId 
AND si.FriendlyName = @FriendlyName

END