CREATE TABLE [dbo].[SearchInstances] (
    [SearchInstanceId] UNIQUEIDENTIFIER NOT NULL,
    [ServiceName]      VARCHAR (255)    NOT NULL,
    [SubscriptionId]   UNIQUEIDENTIFIER NOT NULL,
    [ResourceGroup]    VARCHAR (255)    NOT NULL,
    [Location]         VARCHAR (50)     NOT NULL,
    [PricingTier]      VARCHAR (50)     NOT NULL,
    [Replicas]         INT              NULL,
    [Partitions]       INT              NULL,
    [IsShared]         BIT              CONSTRAINT [DF_SearchInstances_IsShared] DEFAULT ((1)) NOT NULL,
    [Endpoint]         VARCHAR (250)    NULL,
    CONSTRAINT [PK_SearchInstances] PRIMARY KEY CLUSTERED ([SearchInstanceId] ASC),
    CONSTRAINT [FK_SearchInstances_ResourceGroups] FOREIGN KEY ([ResourceGroup]) REFERENCES [dbo].[ResourceGroups] ([ResourceGroup]),
    CONSTRAINT [FK_SearchInstances_Subscriptions] FOREIGN KEY ([SubscriptionId]) REFERENCES [dbo].[Subscriptions] ([SubscriptionId])
);