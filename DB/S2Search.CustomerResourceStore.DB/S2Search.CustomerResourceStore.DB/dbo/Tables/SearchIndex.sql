CREATE TABLE [dbo].[SearchIndex] (
    [SearchIndexId]    UNIQUEIDENTIFIER NOT NULL,
    [CustomerId]       UNIQUEIDENTIFIER NOT NULL,
    [SearchInstanceId] UNIQUEIDENTIFIER NULL,
    [IndexName]        VARCHAR (60)     NOT NULL,
    [FriendlyName]     VARCHAR (100)    NOT NULL,
    [PricingSkuId]     VARCHAR (50)     NOT NULL,
    [CreatedDate]      DATETIME         CONSTRAINT [DF_SearchIndex_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_SearchIndex] PRIMARY KEY CLUSTERED ([SearchIndexId] ASC),
    CONSTRAINT [FK_SearchIndex_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([CustomerId])
);

