CREATE TABLE [dbo].[ServicePrinciples] (
    [ClientId]       UNIQUEIDENTIFIER NOT NULL,
    [Name]           VARCHAR (50)     NOT NULL,
    [TenantId]       UNIQUEIDENTIFIER NOT NULL,
    [SubscriptionId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]    DATETIME         CONSTRAINT [DF_ServicePrinciples_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_ServicePrinciples] PRIMARY KEY CLUSTERED ([ClientId] ASC),
    CONSTRAINT [FK_ServicePrinciples_Subscriptions] FOREIGN KEY ([SubscriptionId]) REFERENCES [dbo].[Subscriptions] ([SubscriptionId])
);

