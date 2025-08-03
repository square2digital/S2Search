USE [CustomerResourceStore]
GO

if not exists(select SkuId from dbo.CustomerPricingTiers where SkuId = 'FREE')
begin
	insert into dbo.CustomerPricingTiers
	  (
	  SkuId,
	  [Name],
	  [Description],
	  EffectiveFromDate
	  )
	  values
	  (
	  'FREE',
	  'S2 Free Tier',
	  'TBD',
	  '2021-03-24 00:00'
	  )
end


if not exists(select SkuId from dbo.CustomerPricingTiers where SkuId = 'FREETRIALMARCH')
begin
	insert into dbo.CustomerPricingTiers
	(
	SkuId,
	[Name],
	[Description],
	EffectiveFromDate,
	EffectiveToDate
	)
	values
	(
	'FREETRIALMARCH',
	'S2 Free Trial March 2021',
	'TBD',
	'2021-03-24 00:00',
	'2021-03-31 23:59:59'
	)
end
 

if not exists(select SkuId from dbo.CustomerPricing where SkuId = 'FREE')
begin
	insert into dbo.CustomerPricing
	(
	SkuId,
	Price,
	EffectiveFromDate
	)
	values
	(
	'FREE',
	0,
	'2021-03-24 00:00'
	)
end


if not exists(select SkuId from dbo.CustomerPricing where SkuId = 'FREETRIALMARCH')
begin
	insert into dbo.CustomerPricing
	(
	SkuId,
	Price,
	EffectiveFromDate,
	EffectiveToDate
	)
	values
	(
	'FREETRIALMARCH',
	0,
	'2021-03-24 00:00',
	'2021-03-31 23:59:59'
	)
end