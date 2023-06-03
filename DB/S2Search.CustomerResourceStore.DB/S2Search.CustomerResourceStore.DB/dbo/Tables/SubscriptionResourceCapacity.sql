CREATE TABLE [dbo].[SubscriptionResourceCapacity] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [SubscriptionId] UNIQUEIDENTIFIER NOT NULL,
    [ResourceType]   VARCHAR (50)     NOT NULL,
    [PricingTier]    VARCHAR (50)     NOT NULL,
    [Quota]          INT              NOT NULL,
    [Used]           INT              NOT NULL,
    [Available]      INT              NOT NULL,
    [ModifiedDate]   DATETIME         NOT NULL,
    CONSTRAINT [PK_SubscriptionResourceCapacity] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SubscriptionResourceCapacity_Subscriptions] FOREIGN KEY ([SubscriptionId]) REFERENCES [dbo].[Subscriptions] ([SubscriptionId])
);

