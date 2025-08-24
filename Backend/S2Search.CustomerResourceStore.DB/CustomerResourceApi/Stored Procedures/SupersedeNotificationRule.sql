CREATE PROCEDURE [CustomerResourceApi].[SupersedeNotificationRule]
(
	@SearchIndexId uniqueidentifier,
	@NotificationRuleId int
)
AS

BEGIN

	UPDATE dbo.NotificationRules
	SET IsLatest = 0,
		SupersededDate = GETUTCDATE()
	WHERE SearchIndexId = @SearchIndexId
	AND NotificationRuleId = @NotificationRuleId
	AND IsLatest = 1
	
END