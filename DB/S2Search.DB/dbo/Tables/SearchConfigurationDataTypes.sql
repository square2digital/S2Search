CREATE TABLE [dbo].[SearchConfigurationDataTypes] (
    [SearchConfigurationDataTypeId] UNIQUEIDENTIFIER NOT NULL,
    [DataType]                      VARCHAR (50)     NOT NULL,
    CONSTRAINT [PK_SearchConfigurationDataTypes] PRIMARY KEY CLUSTERED ([SearchConfigurationDataTypeId] ASC)
);

