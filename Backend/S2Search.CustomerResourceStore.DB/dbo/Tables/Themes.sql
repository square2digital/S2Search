CREATE TABLE [dbo].[Themes] (
    [ThemeId]            UNIQUEIDENTIFIER NOT NULL,
    [PrimaryHexColour]   NVARCHAR (10)    NULL,
    [SecondaryHexColour] NVARCHAR (10)    NULL,
    [NavBarHexColour]    NVARCHAR (10)    NULL,
    [LogoURL]            NVARCHAR (1000)  NULL,
    [MissingImageURL]    NVARCHAR (1000)  NULL,
    [CustomerId]         UNIQUEIDENTIFIER NULL,
    [SearchIndexId]      UNIQUEIDENTIFIER NULL,
    [CreatedDate]        DATETIME         CONSTRAINT [DF_Theme_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]       DATETIME         NULL,
    CONSTRAINT [PK_Themes] PRIMARY KEY CLUSTERED ([ThemeId] ASC),
    CONSTRAINT [FK_Theme_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([CustomerId]),
    CONSTRAINT [FK_Theme_SearchIndex] FOREIGN KEY ([SearchIndexId]) REFERENCES [dbo].[SearchIndex] ([SearchIndexId])
);

