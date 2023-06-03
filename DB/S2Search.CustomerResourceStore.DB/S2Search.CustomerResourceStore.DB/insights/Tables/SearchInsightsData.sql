CREATE TABLE [insights].[SearchInsightsData] (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [SearchIndexId] UNIQUEIDENTIFIER NOT NULL,
    [DataCategory]  VARCHAR (50)     NOT NULL,
    [DataPoint]     VARCHAR (1000)   NOT NULL,
    [Count]         INT              CONSTRAINT [DF_SearchInsightsData_Count] DEFAULT ((0)) NOT NULL,
    [Date]          DATE             CONSTRAINT [DF_SearchInsightsData_Date] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_SearchInsightsData_ModifiedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_SearchInsightsData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

