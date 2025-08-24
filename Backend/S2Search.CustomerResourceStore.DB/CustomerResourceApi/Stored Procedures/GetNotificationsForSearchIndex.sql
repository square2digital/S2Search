CREATE PROCEDURE [CustomerResourceApi].[GetNotificationsForSearchIndex]
(
	@SearchIndexId uniqueidentifier,
	@StartDate datetime,
	@EndDate datetime,
	@RowsToSkip int,
	@PageSize int
)
AS

BEGIN

SELECT
n.NotificationId,
n.[Event],
n.Recipients,
n.TransmitType,
n.CreatedDate
FROM dbo.Notifications n
WHERE n.SearchIndexId = @SearchIndexId
AND n.CreatedDate >= @StartDate
AND n.CreatedDate <= @EndDate
ORDER BY n.CreatedDate DESC
OFFSET @RowsToSkip ROWS
FETCH NEXT @PageSize ROWS ONLY;

SELECT @@ROWCOUNT as TotalCount

END