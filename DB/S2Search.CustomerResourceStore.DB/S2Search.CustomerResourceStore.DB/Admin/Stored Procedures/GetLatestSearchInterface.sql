CREATE PROCEDURE [Admin].[GetLatestSearchInterface]
(
	@SearchIndexId uniqueidentifier
)
AS

BEGIN

SELECT TOP 1
si.SearchInterfaceId,
si.SearchEndpoint,
si.InterfaceType as [Type],
si.LogoURL,
si.BannerStyle,
si.CreatedDate
FROM [dbo].[SearchInterfaces] si
WHERE si.SearchIndexId = @SearchIndexId
AND si.IsLatest = 1

END