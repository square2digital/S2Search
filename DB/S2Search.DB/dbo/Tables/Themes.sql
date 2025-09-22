CREATE TABLE Themes (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [PrimaryHexColour]   NVARCHAR (10)    NULL,
    [SecondaryHexColour] NVARCHAR (10)    NULL,
    [NavBarHexColour]    NVARCHAR (10)    NULL,
    [LogoURL]            NVARCHAR (1000)  NULL,
    [MissingImageURL]    NVARCHAR (1000)  NULL,
    [CustomerId]         UNIQUEIDENTIFIER NULL,
    [SearchIndexId]      UNIQUEIDENTIFIER NULL,
    [CreatedDate]        DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]       DATETIME         NULL,
    CONSTRAINT [PK_Themes] PRIMARY KEY CLUSTERED ([Id] ASC)
);