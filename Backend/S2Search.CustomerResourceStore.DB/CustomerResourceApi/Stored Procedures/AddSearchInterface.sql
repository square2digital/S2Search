﻿CREATE PROCEDURE [CustomerResourceApi].[AddSearchInterface]
(
	@SearchIndexId uniqueidentifier,
	@InterfaceType varchar(50),
	@LogoURL varchar(255),
	@BannerStyle varchar(255)
)
AS

BEGIN

	EXEC [CustomerResourceApi].[SupersedeLatestSearchInterface] @SearchIndexId = @SearchIndexId
	
	INSERT INTO dbo.SearchInterfaces
	(
		SearchIndexId,
		InterfaceType,
		LogoURL,
		BannerStyle
	)
	VALUES
	(
		@SearchIndexId,
		@InterfaceType,
		@LogoURL,
		@BannerStyle
	)

END