CREATE TABLE [dbo].[SearchInstanceKeys] (
    [SearchInstanceKeyId] UNIQUEIDENTIFIER NOT NULL,
    [SearchInstanceId]    UNIQUEIDENTIFIER NOT NULL,
    [KeyType]             VARCHAR (50)     NOT NULL,
    [Name]                VARCHAR (100)    NOT NULL,
    [ApiKey]              VARCHAR (255)    NOT NULL,
    [CreatedDate]         DATETIME         CONSTRAINT [DF_SearchInstanceKeys_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]        DATETIME         NULL,
    [IsLatest]            BIT              CONSTRAINT [DF_SearchInstanceKeys_IsLatest] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_SearchInstanceKeys] PRIMARY KEY CLUSTERED ([SearchInstanceKeyId] ASC),
    CONSTRAINT [FK_SearchInstanceKeys_SearchInstances] FOREIGN KEY ([SearchInstanceId]) REFERENCES [dbo].[SearchInstances] ([SearchInstanceId])
);

