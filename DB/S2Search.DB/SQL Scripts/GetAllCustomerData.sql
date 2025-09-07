DECLARE @CustomerId uniqueidentifier = '98292a56-6049-4e35-aa07-a225dd7f01ee'

SELECT *
FROM dbo.Customers c
INNER JOIN [dbo].[SearchIndex] si ON c.CustomerId = si.CustomerId
INNER JOIN [dbo].[SearchIndexKeys] sik ON si.SearchIndexId = sik.SearchIndexId
INNER JOIN [dbo].[Feeds] f ON f.SearchIndexId = si.SearchIndexId
INNER JOIN [dbo].[SearchInterfaces] sint ON sint.SearchIndexId = si.SearchIndexId
INNER JOIN [dbo].[Themes] t ON t.[SearchIndexId] = si.SearchIndexId
INNER JOIN [dbo].[FeedCredentials] fc ON fc.SearchIndexId = si.SearchIndexId
INNER JOIN [dbo].[SearchConfigurationMappings] scm ON scm.SearchIndexId = si.SearchIndexId

WHERE c.CustomerId = @CustomerId