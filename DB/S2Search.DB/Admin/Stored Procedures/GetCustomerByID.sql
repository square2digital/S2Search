﻿-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Admin].[GetCustomerByID]
	-- Add the parameters for the stored procedure here
	@CustomerId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
	c.[Id],
	c.[BusinessName]
	FROM [dbo].[Customers] c 
	WHERE c.Id = @CustomerId
END