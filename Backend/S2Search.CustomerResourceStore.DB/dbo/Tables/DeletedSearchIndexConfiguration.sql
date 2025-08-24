CREATE TABLE [dbo].[DeletedSearchIndexConfiguration] (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [CustomerId]    UNIQUEIDENTIFIER NOT NULL,
    [SearchIndexId] UNIQUEIDENTIFIER NOT NULL,
    [Configuration] VARCHAR (MAX)    NOT NULL,
    [DeletedDate]   DATETIME         CONSTRAINT [DF_DeletedSearchIndexConfiguration_DeletedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_DeletedSearchIndexConfiguration] PRIMARY KEY CLUSTERED ([Id] ASC)
);

