CREATE TABLE [dbo].[SearchConfigurationMappings] (
    [SearchConfigurationMappingId] UNIQUEIDENTIFIER NOT NULL,
    [Value]                        VARCHAR (MAX)    NOT NULL,
    [SeachConfigurationOptionId]   UNIQUEIDENTIFIER NOT NULL,
    [SearchIndexId]                UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]                  DATETIME         CONSTRAINT [DF_SearchConfigurationMappings_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]                 DATETIME         NULL,
    CONSTRAINT [PK_SearchConfiguration] PRIMARY KEY CLUSTERED ([SearchConfigurationMappingId] ASC)
);

