CREATE TABLE SearchInsightsData (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [SearchIndexId] UNIQUEIDENTIFIER NOT NULL,
    [DataCategory]  VARCHAR (50)     NOT NULL,
    [DataPoint]     VARCHAR (1000)   NOT NULL,
    [Count]         INT              DEFAULT ((0)) NOT NULL,
    [Date]          DATE             DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]  DATETIME         DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_SearchInsightsData] PRIMARY KEY CLUSTERED ([Id] ASC)
);