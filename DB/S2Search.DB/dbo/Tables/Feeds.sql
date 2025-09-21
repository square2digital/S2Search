CREATE TABLE Feeds (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [FeedType]         VARCHAR (20)     NOT NULL,
    [FeedScheduleCron] VARCHAR (255)    NOT NULL,
    [SearchIndexId]    UNIQUEIDENTIFIER NOT NULL,
    [DataFormat]       VARCHAR (50)     NOT NULL,
    [CreatedDate]      DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SupersededDate]   DATETIME         NULL,
    [IsLatest]         BIT              DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Feeds] PRIMARY KEY CLUSTERED ([Id] ASC)
);