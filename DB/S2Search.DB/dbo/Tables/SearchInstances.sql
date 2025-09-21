CREATE TABLE SearchInstances (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [ServiceName]      VARCHAR (255)    NOT NULL,
    [Location]         VARCHAR (50)     NOT NULL,
    [PricingTier]      VARCHAR (50)     NOT NULL,
    [Replicas]         INT              NULL,
    [Partitions]       INT              NULL,
    [IsShared]         BIT              DEFAULT ((1)) NOT NULL,
    [Type]             VARCHAR (255)    NOT NULL,
    [RootEndpoint]     VARCHAR (250)    NULL,
    CONSTRAINT [PK_SearchInstances] PRIMARY KEY CLUSTERED ([Id] ASC)
);