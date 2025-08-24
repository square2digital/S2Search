CREATE TABLE [dbo].[Notifications] (
    [NotificationId] INT              IDENTITY (1, 1) NOT NULL,
    [SearchIndexId]  UNIQUEIDENTIFIER NOT NULL,
    [Recipients]     VARCHAR (255)    NULL,
    [Event]          VARCHAR (100)    NOT NULL,
    [Category]       VARCHAR (100)    CONSTRAINT [DF_Notifications_Category] DEFAULT ('None') NOT NULL,
    [TransmitType]   VARCHAR (100)    NOT NULL,
    [CreatedDate]    DATETIME         CONSTRAINT [DF_Notifications_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Notifications_1] PRIMARY KEY CLUSTERED ([NotificationId] ASC)
);

