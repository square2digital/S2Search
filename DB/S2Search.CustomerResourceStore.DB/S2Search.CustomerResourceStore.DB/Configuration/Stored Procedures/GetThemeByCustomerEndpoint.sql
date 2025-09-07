CREATE PROCEDURE [Configuration].[GetThemeByCustomerEndpoint]
(
	@CustomerEndpoint varchar(250)
)
AS

BEGIN

SELECT
t.ThemeId,
t.PrimaryHexColour,
t.SecondaryHexColour,
t.NavBarHexColour,
t.LogoURL,
t.MissingImageURL
FROM dbo.Themes t
INNER JOIN dbo.SearchInterfaces sui on sui.SearchIndexId = t.SearchIndexId AND sui.IsLatest = 1
WHERE sui.SearchEndpoint = @CustomerEndpoint

END