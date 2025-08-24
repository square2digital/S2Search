CREATE TABLE [dbo].[CustomerPricing] (
    [CustomerPricingId] INT          IDENTITY (1, 1) NOT NULL,
    [SkuId]             VARCHAR (50) NOT NULL,
    [Price]             MONEY        NOT NULL,
    [CreatedDate]       DATETIME     CONSTRAINT [DF_CustomerPricing_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]      DATETIME     NULL,
    [EffectiveFromDate] DATETIME     NOT NULL,
    [EffectiveToDate]   DATETIME     NULL,
    CONSTRAINT [PK_CustomerPricing] PRIMARY KEY CLUSTERED ([CustomerPricingId] ASC),
    CONSTRAINT [FK_CustomerPricing_CustomerPricingTiers] FOREIGN KEY ([SkuId]) REFERENCES [dbo].[CustomerPricingTiers] ([SkuId])
);

