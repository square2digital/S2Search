CREATE PROCEDURE [CustomerResourceApi].[GetNotificationRules]
(
	@SearchIndexId uniqueidentifier
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
AND IsLatest = 1

END