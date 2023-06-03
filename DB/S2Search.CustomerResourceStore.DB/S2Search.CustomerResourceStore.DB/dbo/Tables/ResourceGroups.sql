CREATE TABLE [dbo].[ResourceGroups] (
    [ResourceGroup]  VARCHAR (255)    NOT NULL,
    [SubscriptionId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]    DATETIME         CONSTRAINT [DF_ResourceGroups_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_ResourceGroups] PRIMARY KEY CLUSTERED ([ResourceGroup] ASC),
    CONSTRAINT [FK_ResourceGroups_Subscriptions] FOREIGN KEY ([SubscriptionId]) REFERENCES [dbo].[Subscriptions] ([SubscriptionId])
);

