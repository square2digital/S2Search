
CREATE PROCEDURE [SFTPGoServicesFunc].[AddFeedCredentials]
(
	@SearchIndexId uniqueidentifier,
	@Username varchar(50),
	@PasswordHash varchar(250)
)
AS

BEGIN
	
	INSERT INTO dbo.FeedCredentials
	(
		Id,
		SearchIndexId,
		Username,
		PasswordHash,
		CreatedDate
	)
	VALUES
	(
		NEWID(),
		@SearchIndexId,
		@Username,
		@PasswordHash,
		GETUTCDATE()
	)

END