CREATE TABLE [Synonyms] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Category]       VARCHAR (50)     NULL,           -- Only for generic synonyms
    [SearchIndexId]  UNIQUEIDENTIFIER NULL,           -- Only for specific synonyms
    [KeyWord]        VARCHAR (50)     NULL,           -- Only for specific synonyms
    [SolrFormat]     VARCHAR (MAX)    NOT NULL,
    [CreatedDate]    DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SupersededDate] DATETIME         NULL,
    [IsLatest]       BIT              DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CombinedSynonyms] PRIMARY KEY CLUSTERED ([Id] ASC)
);