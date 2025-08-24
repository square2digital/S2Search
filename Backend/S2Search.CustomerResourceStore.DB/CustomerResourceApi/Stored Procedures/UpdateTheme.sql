
CREATE PROCEDURE [CustomerResourceApi].[UpdateTheme]
	@ThemeId uniqueidentifier,
	@PrimaryHexColour nvarchar(10),
	@SecondaryHexColour nvarchar(10),
	@NavBarHexColour nvarchar(10),
	@LogoURL nvarchar(1000),
	@MissingImageURL nvarchar(1000)
AS
BEGIN

	UPDATE [dbo].[Themes]
	SET PrimaryHexColour = @PrimaryHexColour,
		SecondaryHexColour = @SecondaryHexColour,
		NavBarHexColour = @NavBarHexColour,
		LogoURL = @LogoURL,
		MissingImageURL = @MissingImageURL,
		ModifiedDate = GETUTCDATE()
	WHERE [ThemeId] = @ThemeId

END