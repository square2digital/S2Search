CREATE TABLE [insights].[SearchIndexRequestLog] (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [SearchIndexId] UNIQUEIDENTIFIER NOT NULL,
    [Count]         INT              CONSTRAINT [DF_SearchIndexRequestLog_Count] DEFAULT ((0)) NOT NULL,
    [Date]          DATE             CONSTRAINT [DF_SearchIndexRequestLog_Date] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_SearchIndexRequestLog_ModifiedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_SearchIndexRequestLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

