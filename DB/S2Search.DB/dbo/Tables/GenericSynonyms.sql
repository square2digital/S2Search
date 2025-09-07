CREATE TABLE [dbo].[GenericSynonyms] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Category]       VARCHAR (50)     NOT NULL,
    [SolrFormat]     VARCHAR (MAX)    NOT NULL,
    [CreatedDate]    DATETIME         CONSTRAINT [DF_GenericSynonyms_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [SupersededDate] DATETIME         NULL,
    [IsLatest]       BIT              CONSTRAINT [DF_GenericSynonyms_IsLatest] DEFAULT ((1)) NOT NULL
);

