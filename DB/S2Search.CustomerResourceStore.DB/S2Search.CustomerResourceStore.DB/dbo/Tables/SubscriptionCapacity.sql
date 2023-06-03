CREATE TABLE [dbo].[SubscriptionCapacity] (
    [Id]                      INT              IDENTITY (1, 1) NOT NULL,
    [SubscriptionId]          UNIQUEIDENTIFIER NOT NULL,
    [ResourceGroupsQuota]     INT              NOT NULL,
    [ResourceGroupsUsed]      INT              NOT NULL,
    [ResourceGroupsAvailable] INT              NOT NULL,
    [ModifiedDate]            DATETIME         NOT NULL,
    CONSTRAINT [PK_SubscriptionCapacity] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SubscriptionCapacity_Subscriptions] FOREIGN KEY ([SubscriptionId]) REFERENCES [dbo].[Subscriptions] ([SubscriptionId])
);

