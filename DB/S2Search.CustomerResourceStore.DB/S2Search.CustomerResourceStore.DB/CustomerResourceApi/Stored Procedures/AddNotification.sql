CREATE PROCEDURE [CustomerResourceApi].[AddNotification]
(
	@SearchIndexId uniqueidentifier,
	@Recipients varchar(255),
	@Event varchar(100),
	@Category varchar(100),
	@TransmitType varchar(100)
)
AS

BEGIN

	INSERT INTO dbo.Notifications
	(
		SearchIndexId,
		Recipients,
		[Event],
		Category,
		TransmitType
	)
	VALUES
	(
		@SearchIndexId,
		@Recipients,
		@Event,
		@Category,
		@TransmitType
	)

END