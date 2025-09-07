CREATE TABLE [dbo].[Customers] (
    [CustomerId]   UNIQUEIDENTIFIER NOT NULL,
    [BusinessName] VARCHAR (100)    NULL,
    [CreatedDate]  DATETIME         CONSTRAINT [DF_Customers_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate] DATETIME         NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([CustomerId] ASC)
);

