CREATE PROCEDURE [Admin].[AddSearchIndex]
(
	@SearchIndexId uniqueidentifier,
	@SearchInstanceId uniqueidentifier = NULL,
	@CustomerId	uniqueidentifier,
	@IndexName varchar(60),
	@FriendlyName varchar(100)
)
AS

BEGIN
	
	INSERT INTO dbo.SearchIndex
	(
		[Id],
		[SearchInstanceId],
		[CustomerId],
		[IndexName],
		[FriendlyName],
		[CreatedDate]
	)
	VALUES
	(
		@SearchIndexId,
		@SearchInstanceId,
		@CustomerId,
		@IndexName,
		@FriendlyName,
		GETUTCDATE()
	)

END