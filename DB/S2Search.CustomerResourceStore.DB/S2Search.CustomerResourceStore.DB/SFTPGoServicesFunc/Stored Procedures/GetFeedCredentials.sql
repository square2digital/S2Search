
CREATE PROCEDURE [SFTPGoServicesFunc].[GetFeedCredentials]
(
	@SearchIndexId uniqueidentifier,
	@Username varchar(50)
)
AS

BEGIN
	
	SELECT
	SearchIndexId,
	Username,
	CreatedDate
	FROM [dbo].[FeedCredentials]
	WHERE SearchIndexId = @SearchIndexId
	AND Username = @Username

END