-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Configuration].[GetConfigurationForSearchIndex]
	-- Add the parameters for the stored procedure here
	@SearchIndexId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT

	sco.[SeachConfigurationOptionId],
    scm.[SearchConfigurationMappingId],
	sco.[Key],
	scm.[Value],
	sco.[FriendlyName],
	sco.[Description],
	sct.[DataType],
	sco.[OrderIndex],
	scm.[CreatedDate],
	scm.[ModifiedDate]

	FROM SearchConfigurationOptions sco
	INNER JOIN SearchConfigurationDataTypes sct ON sco.SearchConfigurationDataTypeId = sct.SearchConfigurationDataTypeId
	LEFT JOIN SearchConfigurationMappings scm ON scm.SeachConfigurationOptionId = sco.SeachConfigurationOptionId

	WHERE scm.SearchIndexId = @SearchIndexId
	ORDER BY sco.[OrderIndex]
END