-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Admin].[GetCustomerFull]
	-- Add the parameters for the stored procedure here
	@CustomerId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT c.[CustomerId]
		  ,c.[BusinessName]
	  FROM [dbo].[Customers] c 
	  WHERE c.CustomerId = @CustomerId

	  SELECT [SearchIndexId]
      ,[CustomerId]
      ,[SearchInstanceId]
      ,[IndexName]
      ,[FriendlyName]
      ,[CreatedDate]
	  FROM [dbo].[SearchIndex] s
	  WHERE s.CustomerId = @CustomerId

END