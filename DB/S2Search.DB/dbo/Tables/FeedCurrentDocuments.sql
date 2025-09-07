CREATE TABLE [dbo].[FeedCurrentDocuments] (
    [DocumentId]    VARCHAR (50)     NOT NULL,
    [SearchIndexId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]   DATETIME         CONSTRAINT [DF_FeedCurrentDocuments_CreatedDate] DEFAULT (getutcdate()) NOT NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ix-DocumentIdForSearchIndexId]
    ON [dbo].[FeedCurrentDocuments]([DocumentId] ASC, [SearchIndexId] ASC);

