
CREATE PROCEDURE [SFTPGoServicesFunc].[DeleteFeedCredentials]
(
	@SearchIndexId uniqueidentifier,
	@Username varchar(50)
)
AS

BEGIN
	
	DELETE FROM dbo.FeedCredentials
	WHERE SearchIndexId = @SearchIndexId
	AND Username = @Username

END