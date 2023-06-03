CREATE TABLE [dbo].[CustomerPricingTiers] (
    [SkuId]             VARCHAR (50)  NOT NULL,
    [Name]              VARCHAR (100) NOT NULL,
    [Description]       VARCHAR (255) NOT NULL,
    [CreatedDate]       DATETIME      CONSTRAINT [DF_CustomerPricingTiers_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]      DATETIME      NULL,
    [EffectiveFromDate] DATETIME      NOT NULL,
    [EffectiveToDate]   DATETIME      NULL,
    CONSTRAINT [PK_CustomerPricingTiers] PRIMARY KEY CLUSTERED ([SkuId] ASC)
);

