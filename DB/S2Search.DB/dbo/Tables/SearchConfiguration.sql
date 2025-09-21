CREATE TABLE SearchConfiguration (
    [Id]                           UNIQUEIDENTIFIER NOT NULL,
    [Value]                        VARCHAR (MAX)    NOT NULL,
    [SearchIndexId]                UNIQUEIDENTIFIER NOT NULL,
    [Key]                          VARCHAR (150)    NOT NULL,
    [FriendlyName]                 VARCHAR (500)    NOT NULL,
    [Description]                  VARCHAR (MAX)    NOT NULL,
    [DataType]                     VARCHAR (50)     NOT NULL,
    [OrderIndex]                   INT              NULL,
    [CreatedDate]                  DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]                 DATETIME         NULL,
    CONSTRAINT [PK_SearchConfigurationMappingsCombined] PRIMARY KEY CLUSTERED ([Id] ASC)
);