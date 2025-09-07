CREATE TABLE [dbo].[SearchIndexKeys] (
    [Id]                  INT              IDENTITY (1, 1) NOT NULL,
    [SearchIndexId]       UNIQUEIDENTIFIER NOT NULL,
    [Name]                VARCHAR (100)    NOT NULL,
    [SearchInstanceKeyId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]         DATETIME         CONSTRAINT [DF_SearchIndexKeys_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_SearchIndexKeys] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SearchIndexKeys_SearchIndex] FOREIGN KEY ([SearchIndexId]) REFERENCES [dbo].[SearchIndex] ([SearchIndexId]),
    CONSTRAINT [FK_SearchIndexKeys_SearchInstanceKeys] FOREIGN KEY ([SearchInstanceKeyId]) REFERENCES [dbo].[SearchInstanceKeys] ([SearchInstanceKeyId])
);

