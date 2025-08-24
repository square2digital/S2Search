CREATE TABLE [dbo].[SearchInstanceReservations] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [SearchInstanceId] UNIQUEIDENTIFIER NOT NULL,
    [SearchIndexId]    UNIQUEIDENTIFIER NOT NULL,
    [ReservedDate]     DATETIME         NOT NULL,
    [ExpiryDate]       DATETIME         NOT NULL,
    CONSTRAINT [PK_SearchInstanceReservations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SearchInstanceReservations_SearchInstances] FOREIGN KEY ([SearchInstanceId]) REFERENCES [dbo].[SearchInstances] ([SearchInstanceId])
);

