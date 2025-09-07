CREATE TABLE [dbo].[SearchInterfaces] (
    [SearchInterfaceId] INT              IDENTITY (1, 1) NOT NULL,
    [SearchIndexId]     UNIQUEIDENTIFIER NOT NULL,
    [SearchEndpoint]    VARCHAR (50)     NULL,
    [InterfaceType]     VARCHAR (50)     NOT NULL,
    [LogoURL]           VARCHAR (255)    NULL,
    [BannerStyle]       VARCHAR (255)    NULL,
    [CreatedDate]       DATETIME         CONSTRAINT [DF_SearchInterfaces_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [SupersededDate]    DATETIME         NULL,
    [IsLatest]          BIT              CONSTRAINT [DF_SearchInterfaces_IsLatest] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_SearchInterfaces] PRIMARY KEY CLUSTERED ([SearchInterfaceId] ASC),
    CONSTRAINT [FK_SearchInterfaces_SearchIndex] FOREIGN KEY ([SearchIndexId]) REFERENCES [dbo].[SearchIndex] ([SearchIndexId])
);

