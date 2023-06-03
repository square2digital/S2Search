CREATE PROCEDURE [CustomerResourceApi].[AddNotificationRule]
(
	@SearchIndexId uniqueidentifier,
	@TransmitType varchar(255),
	@Recipients varchar(255),
	@Trigger varchar(255)
)
AS

BEGIN

	INSERT INTO dbo.NotificationRules
	(
		SearchIndexId,
		TransmitType,
		Recipients,
		[Trigger]
	)
	VALUES
	(
		@SearchIndexId,
		@TransmitType,
		@Recipients,
		@Trigger
	)

	SELECT @@IDENTITY as NotificationRuleId

END