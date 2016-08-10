CREATE TABLE [dbo].[KnowledgeBaseKeyword]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[KnowledgeBaseId]	INT				NOT NULL CONSTRAINT [FK_KnowledgeBaseTag_KnowledgeBase] FOREIGN KEY REFERENCES [KnowledgeBase]([Id]),
	[KeywordId]			INT				NOT NULL CONSTRAINT [FK_KnowledgeBaseKeyword_Keyword] FOREIGN KEY REFERENCES [Keyword]([Id]),
	[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO