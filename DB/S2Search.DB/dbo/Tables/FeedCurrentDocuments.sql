CREATE TABLE FeedCurrentDocuments (
    [Id]            VARCHAR (50)     NOT NULL,
    [SearchIndexId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]   DATETIME         DEFAULT (getutcdate()) NOT NULL
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [ix-DocumentIdForSearchIndexId]
    ON [dbo].[FeedCurrentDocuments]([Id] ASC, [SearchIndexId] ASC);