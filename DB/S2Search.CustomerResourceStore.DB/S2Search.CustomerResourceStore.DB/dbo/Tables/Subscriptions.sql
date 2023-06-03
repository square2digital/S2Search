CREATE TABLE [dbo].[Subscriptions] (
    [SubscriptionId] UNIQUEIDENTIFIER NOT NULL,
    [Name]           VARCHAR (50)     NOT NULL,
    [CreatedDate]    DATETIME         CONSTRAINT [DF_Subscriptions_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Subscriptions] PRIMARY KEY CLUSTERED ([SubscriptionId] ASC)
);

