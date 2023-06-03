CREATE TABLE [dbo].[FeedCredentials] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [SearchIndexId] UNIQUEIDENTIFIER NOT NULL,
    [Username]      VARCHAR (50)     NOT NULL,
    [PasswordHash]  VARCHAR (MAX)    NOT NULL,
    [CreatedDate]   DATETIME         CONSTRAINT [DF_FeedCredentials_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]  DATETIME         NULL
);

