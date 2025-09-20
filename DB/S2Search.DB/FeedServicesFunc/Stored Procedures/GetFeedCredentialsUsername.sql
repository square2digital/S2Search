CREATE PROCEDURE [FeedServicesFunc].[GetFeedCredentialsUsername]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

SELECT TOP 1
fc.SearchIndexId,
fc.Username,
fc.CreatedDate,
fc.ModifiedDate
FROM dbo.FeedCredentials fc
WHERE fc.SearchIndexId = @SearchIndexId

END