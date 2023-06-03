CREATE PROCEDURE [FeedServicesFunc].[GetCurrentFeedDocumentsTotal]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

	SELECT 
	COUNT(1) AS TotalDocuments
	FROM [dbo].[FeedCurrentDocuments] 
	WHERE SearchIndexId = @SearchIndexId

END