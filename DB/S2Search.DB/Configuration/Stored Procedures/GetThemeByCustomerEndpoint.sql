CREATE PROCEDURE [Configuration].[GetThemeByCustomerEndpoint]
(
	@CustomerEndpoint varchar(250)
)
AS

BEGIN

SELECT
t.Id,
t.PrimaryHexColour,
t.SecondaryHexColour,
t.NavBarHexColour,
t.LogoURL,
t.MissingImageURL
FROM dbo.Themes t
INNER JOIN dbo.Customers c on t.CustomerId = c.Id
WHERE c.CustomerEndpoint = @CustomerEndpoint

END