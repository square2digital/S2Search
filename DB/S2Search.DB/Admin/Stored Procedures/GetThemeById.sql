-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Admin].[GetThemeById]
	-- Add the parameters for the stored procedure here
	@ThemeId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Id]
      ,[PrimaryHexColour]
      ,[SecondaryHexColour]
      ,[NavBarHexColour]
      ,[LogoURL]
	  ,[MissingImageURL]
      ,[CustomerId]
      ,[SearchIndexId]
      ,[CreatedDate]
      ,[ModifiedDate]
	FROM [dbo].[Themes]
	WHERE [Id] = @ThemeId

END