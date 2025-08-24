CREATE TABLE [dbo].[Feeds] (
    [FeedId]           INT              IDENTITY (1, 1) NOT NULL,
    [FeedType]         VARCHAR (20)     NOT NULL,
    [FeedScheduleCron] VARCHAR (255)    NOT NULL,
    [SearchIndexId]    UNIQUEIDENTIFIER NOT NULL,
    [DataFormat]       VARCHAR (50)     NOT NULL,
    [CreatedDate]      DATETIME         CONSTRAINT [DF_Feeds_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [SupersededDate]   DATETIME         NULL,
    [IsLatest]         BIT              CONSTRAINT [DF_Feeds_IsLatest] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Feeds] PRIMARY KEY CLUSTERED ([FeedId] ASC),
    CONSTRAINT [FK_Feeds_SearchIndex] FOREIGN KEY ([SearchIndexId]) REFERENCES [dbo].[SearchIndex] ([SearchIndexId])
);

