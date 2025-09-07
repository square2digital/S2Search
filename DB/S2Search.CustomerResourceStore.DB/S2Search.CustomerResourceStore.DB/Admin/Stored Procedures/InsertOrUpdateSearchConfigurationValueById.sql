-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Admin].[InsertOrUpdateSearchConfigurationValueById]
	-- Add the parameters for the stored procedure here
	@Value varchar(MAX),
	@SearchConfigurationMappingId uniqueidentifier,
	@SeachConfigurationOptionId uniqueidentifier,
	@SearchIndexId uniqueidentifier
AS
BEGIN

	DECLARE @MappingCount int
	SELECT @MappingCount = COUNT(*)
	FROM [dbo].[SearchConfigurationMappings]
	WHERE SearchConfigurationMappingId = @SearchConfigurationMappingId
	AND SeachConfigurationOptionId = @SeachConfigurationOptionId

    IF (@MappingCount = 0)
		BEGIN
		   INSERT INTO [dbo].[SearchConfigurationMappings]([SearchConfigurationMappingId], [Value], [SeachConfigurationOptionId], [SearchIndexId])
		   VALUES(@SearchConfigurationMappingId, @Value, @SeachConfigurationOptionId, @SearchIndexId)
		END
    IF(@MappingCount = 1)
		BEGIN
			UPDATE [dbo].[SearchConfigurationMappings]
			SET
				[Value] = @Value,
				[ModifiedDate] = GETUTCDATE() 
				WHERE [SearchConfigurationMappingId] = @SearchConfigurationMappingId
		END
END