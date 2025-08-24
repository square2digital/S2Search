﻿-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [CustomerResourceApi].[GetThemeByCustomerId]
	-- Add the parameters for the stored procedure here
	@CustomerId uniqueidentifier
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
	where [CustomerId] = @CustomerId

END