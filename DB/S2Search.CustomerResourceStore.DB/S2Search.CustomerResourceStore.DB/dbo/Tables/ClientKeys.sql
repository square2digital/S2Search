CREATE TABLE [dbo].[ClientKeys] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [ClientId]     UNIQUEIDENTIFIER NOT NULL,
    [Name]         VARCHAR (150)    NOT NULL,
    [Value]        VARCHAR (100)    NOT NULL,
    [ExpiryDate]   DATETIME         NULL,
    [IsLatest]     BIT              CONSTRAINT [DF_ClientKeys_IsLatest] DEFAULT ((1)) NOT NULL,
    [CreatedDate]  DATETIME         CONSTRAINT [DF_ClientKeys_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate] DATETIME         NULL,
    CONSTRAINT [PK_ClientKeys] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ClientKeys_ServicePrinciples] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[ServicePrinciples] ([ClientId])
);

