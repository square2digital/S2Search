CREATE PROCEDURE [SFTPGoServicesFunc].[UpdateFeedCredentials]
(
	@SearchIndexId uniqueidentifier,
	@Username varchar(50),
	@PasswordHash varchar(250)
)
AS

BEGIN
	
	UPDATE dbo.FeedCredentials
	SET	PasswordHash = @PasswordHash,
		ModifiedDate = GETUTCDATE()
	WHERE SearchIndexId = @SearchIndexId
	AND Username = @Username

END