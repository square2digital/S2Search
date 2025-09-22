CREATE TABLE [dbo].[SearchInstanceKeys] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [SearchInstanceId]    UNIQUEIDENTIFIER NOT NULL,
    [KeyType]             VARCHAR (50)     NOT NULL,
    [Name]                VARCHAR (100)    NOT NULL,
    [ApiKey]              VARCHAR (255)    NOT NULL,
    [CreatedDate]         DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]        DATETIME         NULL,
    [IsLatest]            BIT              DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_SearchInstanceKeys]     PRIMARY KEY CLUSTERED ([Id] ASC)
);