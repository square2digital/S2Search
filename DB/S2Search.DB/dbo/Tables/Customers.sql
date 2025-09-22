CREATE TABLE Customers (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [BusinessName] VARCHAR (100)    NULL,
    [CustomerEndpoint] VARCHAR (100)    NULL,
    [CreatedDate]  DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate] DATETIME         NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);