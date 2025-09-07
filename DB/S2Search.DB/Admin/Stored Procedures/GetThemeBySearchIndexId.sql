-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Admin].[GetThemeBySearchIndexId]
	-- Add the parameters for the stored procedure here
	@SearchIndexId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [ThemeId]
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
	where [SearchIndexId] = @SearchIndexId

END