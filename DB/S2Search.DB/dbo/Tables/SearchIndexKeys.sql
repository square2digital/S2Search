CREATE TABLE [dbo].[SearchIndexKeys] (
    [Id]                  INT              IDENTITY (1, 1) NOT NULL,
    [SearchIndexId]       UNIQUEIDENTIFIER NOT NULL,
    [Name]                VARCHAR (100)    NOT NULL,
    [SearchInstanceKeyId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]         DATETIME         CONSTRAINT [DF_SearchIndexKeys_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_SearchIndexKeys] PRIMARY KEY CLUSTERED ([Id] ASC)
);

