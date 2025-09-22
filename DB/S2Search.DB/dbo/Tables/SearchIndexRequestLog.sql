CREATE TABLE SearchIndexRequestLog (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [SearchIndexId] UNIQUEIDENTIFIER NOT NULL,
    [Count]         INT              DEFAULT ((0)) NOT NULL,
    [Date]          DATE             DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]  DATETIME         DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_SearchIndexRequestLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);