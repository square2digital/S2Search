CREATE TABLE [dbo].[Synonyms] (
    [SynonymId]      UNIQUEIDENTIFIER NOT NULL,
    [SearchIndexId]  UNIQUEIDENTIFIER NOT NULL,
    [KeyWord]        VARCHAR (50)     NOT NULL,
    [SolrFormat]     VARCHAR (MAX)    NOT NULL,
    [CreatedDate]    DATETIME         CONSTRAINT [DF_Synonyms_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [SupersededDate] DATETIME         NULL,
    [IsLatest]       BIT              CONSTRAINT [DF_Synonyms_IsLatest] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Synonyms] PRIMARY KEY CLUSTERED ([SynonymId] ASC),
    CONSTRAINT [FK_Synonyms_SearchIndex] FOREIGN KEY ([SearchIndexId]) REFERENCES [dbo].[SearchIndex] ([SearchIndexId])
);





