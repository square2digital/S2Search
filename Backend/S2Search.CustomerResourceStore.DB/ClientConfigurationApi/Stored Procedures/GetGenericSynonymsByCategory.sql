CREATE PROCEDURE [ClientConfigurationApi].[GetGenericSynonymsByCategory]
(
@Category varchar(50)
)
AS

BEGIN

SELECT
Id,
Category,
SolrFormat,
CreatedDate
FROM [dbo].[GenericSynonyms]
WHERE Category = @Category
AND IsLatest = 1

END