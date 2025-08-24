CREATE TABLE [dbo].[ResourceGroupsCapacity] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [ResourceGroup]      VARCHAR (255) NOT NULL,
    [ResourcesQuota]     INT           NOT NULL,
    [ResourcesUsed]      INT           NOT NULL,
    [ResourcesAvailable] INT           NOT NULL,
    [ModifiedDate]       DATETIME      NOT NULL,
    CONSTRAINT [PK_ResourceGroupsCapacity] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResourceGroupsCapacity_ResourceGroups] FOREIGN KEY ([ResourceGroup]) REFERENCES [dbo].[ResourceGroups] ([ResourceGroup])
);

