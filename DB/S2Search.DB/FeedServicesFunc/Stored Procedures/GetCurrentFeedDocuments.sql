CREATE PROCEDURE [FeedServicesFunc].[GetCurrentFeedDocuments]
(
	@SearchIndexId uniqueidentifier,
	@PageNumber int,
	@PageSize int
)
AS

BEGIN

	SELECT
	[Id]
	FROM [dbo].[FeedCurrentDocuments]
	WHERE SearchIndexId = @SearchIndexId
	ORDER BY CreatedDate ASC
	OFFSET (@PageNumber - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY;

END