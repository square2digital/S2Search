CREATE TYPE [SearchInsightsFunc].[SearchInsightsData] AS TABLE (
    [DataCategory] VARCHAR (50)   NOT NULL,
    [DataPoint]    VARCHAR (1000) NOT NULL,
    [Date]         DATE           NOT NULL);