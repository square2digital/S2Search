CREATE TABLE [dbo].[SearchInstanceCapacity] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [SearchInstanceId] UNIQUEIDENTIFIER NOT NULL,
    [StorageQuotaMB]   DECIMAL (18, 2)  NOT NULL,
    [IndexesQuota]     INT              NOT NULL,
    [StorageUsedMB]    DECIMAL (18, 2)  NOT NULL,
    [IndexesUsed]      INT              NOT NULL,
    [IndexesReserved]  INT              NOT NULL,
    [IndexesAvailable] INT              NOT NULL,
    [DocumentsQuota]   INT              NULL,
    [DocumentsUsed]    NCHAR (10)       NOT NULL,
    [ModifiedDate]     DATETIME         NOT NULL,
    CONSTRAINT [PK_SearchInstanceCapacity] PRIMARY KEY CLUSTERED ([Id] ASC)
);

