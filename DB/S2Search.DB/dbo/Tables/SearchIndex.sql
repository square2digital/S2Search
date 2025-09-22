CREATE TABLE SearchIndex (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [CustomerId]       UNIQUEIDENTIFIER NOT NULL,
    [SearchInstanceId] UNIQUEIDENTIFIER NULL,
    [IndexName]        VARCHAR (60)     NOT NULL,
    [FriendlyName]     VARCHAR (100)    NOT NULL,
    [PricingSkuId]     VARCHAR (50)     NOT NULL,
    [CreatedDate]      DATETIME         DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_SearchIndex] PRIMARY KEY CLUSTERED ([Id] ASC)
);