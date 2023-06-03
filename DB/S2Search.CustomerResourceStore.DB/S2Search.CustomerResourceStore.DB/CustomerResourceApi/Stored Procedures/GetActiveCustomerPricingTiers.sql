CREATE PROCEDURE [CustomerResourceApi].[GetActiveCustomerPricingTiers]
AS

BEGIN

DECLARE @Now DATETIME = GETUTCDATE()
DECLARE @Tomorrow DATETIME = DATEADD(DAY, 1, @Now)

SELECT
cpt.SkuId,
cpt.[Name],
cpt.[Description],
cp.Price
FROM dbo.CustomerPricingTiers cpt
INNER JOIN dbo.CustomerPricing cp on cp.SkuId = cpt.SkuId AND @Now BETWEEN cp.EffectiveFromDate AND ISNULL(cp.EffectiveToDate, @Tomorrow)
WHERE @Now BETWEEN cpt.EffectiveFromDate AND ISNULL(cpt.EffectiveToDate, @Tomorrow)

END