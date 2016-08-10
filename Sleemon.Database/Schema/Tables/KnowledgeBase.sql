CREATE TABLE [dbo].[KnowledgeBase]
(
	[Id]				INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Title]				NVARCHAR(200)	NOT NULL,
	[Detail]			NVARCHAR(MAX)	NOT NULL,
	[KnowledgeCategory]	INT				NOT NULL, --1. 陈列。 2. 销售
	[LastUpdateUser]	NVARCHAR(200)	NOT NULL,
	[LastUpdateTime]	DATETIME		NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive]			BIT				NOT NULL DEFAULT(1)
)
GO

--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容只限文本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KnowledgeBase', @level2type=N'COLUMN',@level2name=N'Detail'
--GO