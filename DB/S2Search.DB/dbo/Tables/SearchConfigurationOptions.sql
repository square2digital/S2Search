CREATE TABLE [dbo].[SearchConfigurationOptions] (
    [SeachConfigurationOptionId]    UNIQUEIDENTIFIER NOT NULL,
    [Key]                           VARCHAR (150)    NOT NULL,
    [FriendlyName]                  VARCHAR (500)    NOT NULL,
    [Description]                   VARCHAR (MAX)    NOT NULL,
    [SearchConfigurationDataTypeId] UNIQUEIDENTIFIER NOT NULL,
    [OrderIndex]                    INT              CONSTRAINT [DF_SearchConfigurationOptions_OrderIndex] DEFAULT (NULL) NULL,
    [CreatedDate]                   DATETIME         CONSTRAINT [DF_SearchConfigurationOptions_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]                  DATETIME         NULL,
    CONSTRAINT [PK_SearchConfigurationOptions] PRIMARY KEY CLUSTERED ([SeachConfigurationOptionId] ASC)
);

