CREATE PROCEDURE [Admin].[GetNotificationRuleById]
(
	@SearchIndexId uniqueidentifier,
	@NotificationRuleId int
)
AS

BEGIN

SELECT
NotificationRuleId,
SearchIndexId,
TransmitType,
Recipients,
[Trigger] as TriggerType,
CreatedDate
FROM dbo.NotificationRules
WHERE SearchIndexId = @SearchIndexId
AND NotificationRuleId = @NotificationRuleId
AND IsLatest = 1

END