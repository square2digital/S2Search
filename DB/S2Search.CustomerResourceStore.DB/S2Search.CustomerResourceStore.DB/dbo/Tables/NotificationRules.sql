CREATE TABLE [dbo].[NotificationRules] (
    [NotificationRuleId] INT              IDENTITY (1, 1) NOT NULL,
    [SearchIndexId]      UNIQUEIDENTIFIER NOT NULL,
    [TransmitType]       VARCHAR (255)    NOT NULL,
    [Recipients]         VARCHAR (255)    NOT NULL,
    [Trigger]            VARCHAR (255)    NOT NULL,
    [CreatedDate]        DATETIME         CONSTRAINT [DF_NotificationRules_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [SupersededDate]     DATETIME         NULL,
    [IsLatest]           BIT              CONSTRAINT [DF_NotificationRules_IsLatest] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([NotificationRuleId] ASC),
    CONSTRAINT [FK_NotificationRules_SearchIndex] FOREIGN KEY ([SearchIndexId]) REFERENCES [dbo].[SearchIndex] ([SearchIndexId])
);

